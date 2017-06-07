using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Leoxia.Commands.Win32
{

    // From http://improve.dk/finding-specific-windows/
    public class WindowsFinder
    {
        private IntPtr _parentHandle;
        private readonly int _childProcessId;
        private IntPtr _childHandle;
        private readonly List<IntPtr> _childHandles = new List<IntPtr>();
        private readonly int _currentProcessId;

        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        // EnumChildWindows works just like EnumWindows, except we can provide a parameter that specifies the parent
        // window handle. If this is NULL or zero, it works just like EnumWindows. Otherwise it'll only return windows
        // whose parent window handle matches the hWndParent parameter.
        [DllImport("user32.Dll")]
        private static extern Boolean EnumChildWindows(IntPtr hWndParent, PChildCallBack lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumWindowStations(PCallBack callback, IntPtr extraData);

        // This is the delegate that sets the signature for the callback function of the EnumWindows function.
        private delegate bool PCallBack(IntPtr hWnd, IntPtr lParam);

        // The PChildCallBack delegate that we used with EnumWindows.
        private delegate bool PChildCallBack(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        static extern IntPtr FindWindowEx(IntPtr hwndParent,
            IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public WindowsFinder(IntPtr parentHandle, int childProcessId)
        {
            _childProcessId = childProcessId;
            _currentProcessId = Process.GetCurrentProcess().Id;
        }

        public IntPtr FindChildWindows()
        {
            // Invoke the EnumChildWindows function.
            //EnumChildWindows(_parentHandle, OnChildWindowHandle, IntPtr.Zero);
            EnumWindowStations(OnWindowHandle, IntPtr.Zero);

            EnumChildWindows(_parentHandle, OnChildWindowHandle, IntPtr.Zero);
            if (_childHandle != IntPtr.Zero)
            {
                return _childHandle;
            }
            //var handle = Process.GetCurrentProcess().SafeHandle.DangerousGetHandle();
            //IntPtr prevChild = IntPtr.Zero;
            //int ct = 0;
            //int maxCount = 100;
            //while (ct < maxCount)
            //{                
            //    IntPtr currChild = FindWindowEx(handle, prevChild, null, null);
            //    if (currChild == IntPtr.Zero) break;
            //    prevChild = currChild;
            //    ++ct;
            //}
            return _childHandle;
        }

        // This function gets called each time a window is found by the EnumChildWindows function. The foun windows here
        // are NOT the final found windows as the only filtering done by EnumChildWindows is on the parent window handle.
        private bool OnChildWindowHandle(IntPtr handle, IntPtr lParam)
        {
            int processId;
            GetWindowThreadProcessId(handle, out processId);
            if (processId == _childProcessId)
            {
                _childHandle = handle;
                _childHandles.Add(handle);
            }
            return false;
        }

        private bool OnWindowHandle(IntPtr handle, IntPtr lParam)
        {
            int processId;
            GetWindowThreadProcessId(handle, out processId);
            if (processId == _currentProcessId)
            {
                _parentHandle = handle;
            }
            return false;
        }

    }
}
