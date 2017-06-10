namespace Leoxia.ReadLine
{
    public class CompletionListNavigator
    {
        private readonly string[] _completionList;
        private int _currentIndex;


        public CompletionListNavigator(string[] completionList)
        {
            _completionList = completionList;
            _currentIndex = 0;
        }

        public string GoNext()
        {
            _currentIndex++;
            return _completionList[_currentIndex];
        }

        public string GoPrevious()
        {
            _currentIndex--;
            return _completionList[_currentIndex];
        }
    }
}