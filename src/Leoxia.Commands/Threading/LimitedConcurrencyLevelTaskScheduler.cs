using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Leoxia.Commands.Threading
{
    // From https://msdn.microsoft.com/fr-fr/library/system.threading.tasks.taskscheduler%28v=vs.110%29.aspx
    // From https://msdn.microsoft.com/library/system.threading.tasks.taskscheduler.aspx
    // Note that we replaced ThreadPool.UnsafeQueueUserWorkItem by Task.Run which is not optimal
    /// <summary>
    ///     Provides a task scheduler that ensures a maximum concurrency level while
    ///     running on top of the thread pool.
    /// </summary>
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        [ThreadStatic] private static bool _currentThreadIsProcessingItems;

        // The maximum concurrency level allowed by this scheduler. 
        private readonly int _maxDegreeOfParallelism;

        // The list of tasks to be executed 
        private readonly ConcurrentQueue<Task> _tasks = new ConcurrentQueue<Task>();

        // Indicates whether the scheduler is currently processing work items. 
        private int _delegatesQueuedOrRunning;

        // Creates a new instance with the specified degree of parallelism. 
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        // Gets the maximum concurrency level supported by this scheduler. 
        public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;

        // Queues a task to the scheduler. 
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough 
            // delegates currently queued or running to process tasks, schedule another. 
            _tasks.Enqueue(task);
            if (Interlocked.CompareExchange(ref _delegatesQueuedOrRunning, _maxDegreeOfParallelism,
                    _maxDegreeOfParallelism) != _maxDegreeOfParallelism)
            {
                Interlocked.Increment(ref _delegatesQueuedOrRunning);
                NotifyThreadPoolOfPendingWork();
            }
        }


        // Inform the ThreadPool that there's work to be executed for this scheduler. 
        private void NotifyThreadPoolOfPendingWork()
        {
            Task.Run(() =>
            {
                // Note that the current thread is now processing work items.
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue.
                    while (true)
                    {
                        Task item;
                        // When there are no more items to be processed,
                        // note that we're done processing, and get out.
                        if (_tasks.Count == 0)
                        {
                            Interlocked.Decrement(ref _delegatesQueuedOrRunning);
                            break;
                        }
                        // Get the next item from the queue
                        if (_tasks.TryDequeue(out item))
                            TryExecuteTask(item);
                        else
                            Task.Delay(10);
                    }
                }
                // We're done processing items on the current thread
                finally
                {
                    _currentThreadIsProcessingItems = false;
                }
            });
        }


        // Attempts to execute the specified task on the current thread. 
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining
            if (!_currentThreadIsProcessingItems) return false;

            // If the task was previously queued, remove it from the queue
            if (taskWasPreviouslyQueued)
                // Try to run the task. 
                if (TryDequeue(task))
                    return TryExecuteTask(task);
                else
                    return false;
            return TryExecuteTask(task);
        }

        // Attempt to remove a previously scheduled task from the scheduler. 
        protected sealed override bool TryDequeue(Task task)
        {
            return _tasks.TryDequeue(out task);
        }

        // Gets an enumerable of the tasks currently scheduled on this scheduler. 
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks;
        }
    }
}