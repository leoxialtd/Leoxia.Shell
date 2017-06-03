using System.Threading;

namespace Leoxia.ReadLine
{
    public interface IHistoryNavigator
    {
        void GoNext();
        void GoPrevious();
        bool HasPrevious { get; }
        bool HasNext { get; }
        CommandLineBuffer Current { get; }
        string Validate();
    }
}