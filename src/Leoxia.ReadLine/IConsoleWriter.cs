using System.Text;

namespace Leoxia.ReadLine
{
    public interface IConsoleWriter
    {
        void Reset();
        void Write(CommandLineBuffer buffer);
        void Backspace(CommandLineBuffer buffer);
        void MoveCursorLeft();
        void MoveCursorRight();
        void MoveCursorHome();
        void MoveCursorEnd();
        void Write(CommandLineBuffer current, char keyChar);
    }
}