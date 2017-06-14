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
                //_input.Write(key.KeyChar);
                var bytes = Encoding.UTF8.GetBytes(new char[] {key.KeyChar,
                        //'\r', 
                        //'\n'
                });
                var res = _baseStream.BeginWrite(bytes, 0, bytes.Length, null, null);
                _baseStream.EndWrite(res);
                _baseStream.Flush(true);
            //}
        }
    }
}