using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Leoxia.Commands.Win32;

namespace Leoxia.Commands.External
{
    public class StreamWriterBridge
    {
        private readonly StreamWriter _input;
        private readonly IntPtr _windowHandle;
        private readonly FileStream _baseStream;


        public StreamWriterBridge(Process process)
        {
            var process1 = process;
            _input = process1.StandardInput;
            _input.AutoFlush = true;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var windowFinder = new WindowsFinder(
                    process1.SafeHandle.DangerousGetHandle(),
                    process1.Id);
                _windowHandle = windowFinder.FindChildWindows();
            }
            _baseStream = (FileStream)_input.BaseStream;
        }

        public void Write(ConsoleKeyInfo key)
        {
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    NativeMethods.WriteToConsole(_windowHandle, key);
            //}
            //else
            //{
                //_input.WriteAsync("" + key.KeyChar);
                var bytes = Encoding.Unicode.GetBytes(new char[] {key.KeyChar, '\r', '\n'});
                _baseStream.Write(bytes, 0, bytes.Length);
            //}
        }
    }
}