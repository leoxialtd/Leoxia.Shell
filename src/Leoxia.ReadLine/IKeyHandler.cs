using System;

namespace Leoxia.ReadLine
{
    public interface IKeyHandler
    {
        void Handle(ConsoleKeyInfo keyInfo);
    }
}