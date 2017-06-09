using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Leoxia.CommandTransform
{
    public class CommandTransformPipeline : ITransformPipeline
    {
        private readonly List<ICommandTransformPipe> _pipelines = new List<ICommandTransformPipe>();

        public CommandTransformPipeline(ICommandTransformPipe[] pipes)
        {
            _pipelines.AddRange(pipes);
        }

        public string Transform(string commandLine)
        {
            var res = commandLine;
            foreach (var pipeline in _pipelines)
            {
                res = pipeline.Transform(res);
            }
            return res;
        }
    }

    public interface ITransformPipeline : ICommandTransformPipe
    {
        
    }
}
