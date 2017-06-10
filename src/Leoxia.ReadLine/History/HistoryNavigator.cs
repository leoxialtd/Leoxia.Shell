using System.Collections.Generic;
using System.Linq;

namespace Leoxia.ReadLine
{
    public class HistoryNavigator : IHistoryNavigator
    {
        private readonly List<string> _history = new List<string>();
        private List<CommandLineBuffer> _buffers = new List<CommandLineBuffer>();
        private int _currentIndex;

        public HistoryNavigator()
        {
            Reset();
        }

        private void Reset()
        {
            _buffers = _history.Select(h => new CommandLineBuffer(h)).ToList();
            _buffers.Add(new CommandLineBuffer());
            _currentIndex = _buffers.Count - 1;
        }

        public string Validate()
        {
            _history.Add(Current.ToString());            
            var result = _buffers[_currentIndex];
            Reset();
            return result.ToString();
        }

        public void GoNext()
        {
            if (_currentIndex + 1 < _buffers.Count)
            {
                _currentIndex++;
            }
        }

        public void GoPrevious()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
            }
        }

        public bool HasPrevious => _buffers.Count > 0 && _currentIndex >= 0;
        public bool HasNext => _currentIndex >= 0 && _currentIndex  < _buffers.Count;
        public CommandLineBuffer Current =>  _buffers[_currentIndex];
    }
}