using System;

namespace Leoxia.ReadLine
{
    using static System.ConsoleKey;
    using static System.ConsoleModifiers;

    public class ControlSequences
    {
        public static readonly ControlSequence ControlB = new ControlSequence(Control, B);
        public static readonly ControlSequence LeftArrow = new ControlSequence(ConsoleKey.LeftArrow);
        public static readonly ControlSequence ControlF = new ControlSequence(Control, F);
        public static readonly ControlSequence RightArrow = new ControlSequence(ConsoleKey.RightArrow);
        public static readonly ControlSequence Home = new ControlSequence(ConsoleKey.Home);
        public static readonly ControlSequence End = new ControlSequence(ConsoleKey.End);
        public static readonly ControlSequence ControlA = new ControlSequence(Control, A);
        public static readonly ControlSequence ControlE = new ControlSequence(Control, E);
        public static readonly ControlSequence ControlH = new ControlSequence(Control, H);
        public static readonly ControlSequence ControlL = new ControlSequence(Control, L);
        public static readonly ControlSequence Backspace = new ControlSequence(ConsoleKey.Backspace);
        public static readonly ControlSequence ControlP = new ControlSequence(Control, P);
        public static readonly ControlSequence ControlN = new ControlSequence(Control, N);
        public static readonly ControlSequence UpArrow = new ControlSequence(ConsoleKey.UpArrow);
        public static readonly ControlSequence DownArrow = new ControlSequence(ConsoleKey.DownArrow);

        private ControlSequences()
        {            
        }
    }
}