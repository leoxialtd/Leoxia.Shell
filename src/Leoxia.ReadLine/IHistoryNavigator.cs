using System.Threading;

namespace Leoxia.ReadLine
{
    public interface IHistoryNavigator
    {
        CommandLineBuffer GetNext();
        CommandLineBuffer GetPrevious();
        bool HasHistory { get; }
        bool HasNext { get; }
        CommandLineBuffer Current { get; }
        string Validate();
    }
}