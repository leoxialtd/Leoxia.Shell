using System.Linq;

namespace Leoxia.ReadLine
{
    public interface ICompletionNavigator
    {
        string[] PreviousAutoComplete(CommandLineBuffer currentCommandLine);
        string[] NextAutoComplete(CommandLineBuffer currentCommandLine);
        void ExitCompletion();
    }
}