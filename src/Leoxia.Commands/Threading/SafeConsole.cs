using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Leoxia.Abstractions.IO;
using Leoxia.Implementations.IO;

namespace Leoxia.Commands.Threading
{


    /// <summary>
    /// Console Adapter that ensure that every parrallel call is forwarded 
    /// to one thread. Ensure synchronization of block of codes, like changing back and forth
    /// the color of the foreground.
    /// </summary>
    /// <seealso cref="IConsole" />
    public class SafeConsoleAdapter : ISafeConsole
    {
        private readonly IConsole _consoleImplementation;
        private readonly INotClsConsole _notClsConsole;
        private readonly TaskFactory _taskFactory;
        private static readonly Lazy<SafeConsoleAdapter> _instance =
            new Lazy<SafeConsoleAdapter>(() => new SafeConsoleAdapter(new ConsoleAdapter()));

        public SafeConsoleAdapter(IConsole consoleImplementation)
        {
            _consoleImplementation = consoleImplementation;
            _notClsConsole = consoleImplementation as INotClsConsole;
            var taskScheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            _taskFactory = new TaskFactory(taskScheduler);
        }

        /// <summary>Gets or sets the background color of the console.</summary>
        /// <returns>
        ///     A value that specifies the background color of the console; that is, the color that appears behind each
        ///     character. The default is black.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     The color specified in a set operation is not a valid member of
        ///     <see cref="T:System.ConsoleColor" />.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Window="SafeTopLevelWindows" />
        /// </PermissionSet>
        public ConsoleColor BackgroundColor
        {
            get => _consoleImplementation.BackgroundColor;
            set => _taskFactory.StartNew(() => { _consoleImplementation.BackgroundColor = value; });
        }

        public void Beep(int frequency, int duration)
        {
            _taskFactory.StartNew(() =>
            {
                _consoleImplementation.Beep(frequency, duration);
            });
        }

        public void Beep()
        {
            _taskFactory.StartNew(() =>
            {
                _consoleImplementation.Beep();
            });
        }

        public int BufferHeight
        {
            get => _consoleImplementation.BufferHeight;
            set =>
                _taskFactory.StartNew(() => { _consoleImplementation.BufferHeight = value; });
        }

        public int BufferWidth
        {
            get => _consoleImplementation.BufferWidth;
            set => _taskFactory.StartNew(() => { _consoleImplementation.BufferWidth = value; });
        }

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => _taskFactory.StartNew(() => { _consoleImplementation.CancelKeyPress += value; });
            remove => _taskFactory.StartNew(() =>
            {
                _consoleImplementation.CancelKeyPress -= value;
            });
        }

        public bool CapsLock
        {
            get => _consoleImplementation.CapsLock;
        }

        public void Clear()
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Clear(); });
        }

        public int CursorLeft
        {
            get => _consoleImplementation.CursorLeft;
            set => _taskFactory.StartNew(() => { _consoleImplementation.CursorLeft = value; });
        }

        public int CursorSize
        {
            get => _consoleImplementation.CursorSize;
            set => _taskFactory.StartNew(() => { _consoleImplementation.CursorSize = value; });
        }

        public int CursorTop
        {
            get => _consoleImplementation.CursorTop;
            set => _taskFactory.StartNew(() => { _consoleImplementation.CursorTop = value; });
        }

        public bool CursorVisible
        {
            get => _consoleImplementation.CursorVisible;
            set => _taskFactory.StartNew(() => { _consoleImplementation.CursorVisible = value; });
        }

        /// <summary>Gets the standard error output stream.</summary>
        /// <returns>A <see cref="T:System.IO.TextWriter" /> that represents the standard error output stream.</returns>
        /// <filterpriority>1</filterpriority>
        public ITextWriter Error
        {
            get => _consoleImplementation.Error;
        }

        /// <summary>Gets or sets the foreground color of the console.</summary>
        /// <returns>
        ///     A <see cref="T:System.ConsoleColor" /> that specifies the foreground color of the console; that is, the color
        ///     of each character that is displayed. The default is gray.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     The color specified in a set operation is not a valid member of
        ///     <see cref="T:System.ConsoleColor" />.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Window="SafeTopLevelWindows" />
        /// </PermissionSet>
        public ConsoleColor ForegroundColor
        {
            get => _consoleImplementation.ForegroundColor;
            set => _taskFactory.StartNew(() => { _consoleImplementation.ForegroundColor = value; });
        }

        /// <summary>Gets the standard input stream.</summary>
        /// <returns>A <see cref="T:System.IO.TextReader" /> that represents the standard input stream.</returns>
        /// <filterpriority>1</filterpriority>
        public ITextReader In
        {
            get => _consoleImplementation.In;
        }

        public Encoding InputEncoding
        {
            get => _consoleImplementation.InputEncoding;
            set => _taskFactory.StartNew(() => { _consoleImplementation.InputEncoding = value; });
        }

        public bool IsErrorRedirected
        {
            get => _consoleImplementation.IsErrorRedirected;
        }

        public bool IsInputRedirected
        {
            get => _consoleImplementation.IsInputRedirected;
        }

        public bool IsOutputRedirected
        {
            get => _consoleImplementation.IsOutputRedirected;
        }

        public bool KeyAvailable
        {
            get => _consoleImplementation.KeyAvailable;
        }

        public int LargestWindowHeight
        {
            get => _consoleImplementation.LargestWindowHeight;
        }

        public int LargestWindowWidth
        {
            get => _consoleImplementation.LargestWindowWidth;
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
            int targetTop)
        {
            _taskFactory.StartNew(() =>
            {
                _consoleImplementation.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft,
                    targetTop);
            });
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
            int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            _taskFactory.StartNew(() =>
            {
                _consoleImplementation.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft,
                    targetTop, sourceChar, sourceForeColor, sourceBackColor);
            });
        }

        public bool NumberLock
        {
            get => _consoleImplementation.NumberLock;
        }

        /// <summary>Acquires the standard error stream.</summary>
        /// <returns>The standard error stream.</returns>
        /// <filterpriority>1</filterpriority>
        public Stream OpenStandardError()
        {
            return _consoleImplementation.OpenStandardError();
        }

        /// <summary>Acquires the standard input stream.</summary>
        /// <returns>The standard input stream.</returns>
        /// <filterpriority>1</filterpriority>
        public Stream OpenStandardInput()
        {
            return _consoleImplementation.OpenStandardInput();
        }

        /// <summary>Acquires the standard output stream.</summary>
        /// <returns>The standard output stream.</returns>
        /// <filterpriority>1</filterpriority>
        public Stream OpenStandardOutput()
        {
            return _consoleImplementation.OpenStandardOutput();
        }

        /// <summary>Gets the standard output stream.</summary>
        /// <returns>A <see cref="T:System.IO.TextWriter" /> that represents the standard output stream.</returns>
        /// <filterpriority>1</filterpriority>
        public ITextWriter Out
        {
            get => _consoleImplementation.Out;
        }

        public Encoding OutputEncoding
        {
            get => _consoleImplementation.OutputEncoding;
            set => _taskFactory.StartNew(() => { _consoleImplementation.OutputEncoding = value; });
        }

        /// <summary>Reads the next character from the standard input stream.</summary>
        /// <returns>
        ///     The next character from the input stream, or negative one (-1) if there are currently no more characters to be
        ///     read.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public int Read()
        {
            return _consoleImplementation.Read();
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return _consoleImplementation.ReadKey(intercept);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return _consoleImplementation.ReadKey();
        }

        /// <summary>Reads the next line of characters from the standard input stream.</summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.OutOfMemoryException">
        ///     There is insufficient memory to allocate a buffer for the returned
        ///     string.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     The number of characters in the next line of characters is
        ///     greater than <see cref="F:System.Int32.MaxValue" />.
        /// </exception>
        /// <filterpriority>1</filterpriority>
        public string ReadLine()
        {
            return _consoleImplementation.ReadLine();
        }

        /// <summary>Sets the foreground and background console colors to their defaults.</summary>
        /// <exception cref="T:System.Security.SecurityException">The user does not have permission to perform this action. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Window="SafeTopLevelWindows" />
        /// </PermissionSet>
        public void ResetColor()
        {
            _taskFactory.StartNew(() => { _consoleImplementation.ResetColor(); });
        }

        public void SetBufferSize(int width, int height)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.SetBufferSize(width, height); });
        }

        public void SetCursorPosition(int left, int top)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.SetCursorPosition(left, top); });
        }

        /// <summary>
        ///     Sets the <see cref="P:System.Console.Error" /> property to the specified <see cref="T:System.IO.TextWriter" />
        ///     object.
        /// </summary>
        /// <param name="newError">A stream that is the new standard error output. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="newError" /> is null.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public void SetError(TextWriter newError)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.SetError(newError); });
        }

        /// <summary>
        ///     Sets the <see cref="P:System.Console.In" /> property to the specified <see cref="T:System.IO.TextReader" />
        ///     object.
        /// </summary>
        /// <param name="newIn">A stream that is the new standard input. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="newIn" /> is null.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public void SetIn(TextReader newIn)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.SetIn(newIn); });
        }

        /// <summary>
        ///     Sets the <see cref="P:System.Console.Out" /> property to the specified <see cref="T:System.IO.TextWriter" />
        ///     object.
        /// </summary>
        /// <param name="newOut">A stream that is the new standard output. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="newOut" /> is null.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public void SetOut(TextWriter newOut)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.SetOut(newOut); });
        }

        public void SetWindowPosition(int left, int top)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.SetWindowPosition(left, top); });
        }

        public void SetWindowSize(int width, int height)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.SetWindowSize(width, height); });
        }

        public string Title
        {
            get => _consoleImplementation.Title;
            set => _taskFactory.StartNew(() => { _consoleImplementation.Title = value; });
        }

        public bool TreatControlCAsInput
        {
            get => _consoleImplementation.TreatControlCAsInput;
            set => _taskFactory.StartNew(() => { _consoleImplementation.TreatControlCAsInput = value; });
        }

        public int WindowHeight
        {
            get => _consoleImplementation.WindowHeight;
            set => _taskFactory.StartNew(() => { _consoleImplementation.WindowHeight = value; });
        }

        public int WindowLeft
        {
            get => _consoleImplementation.WindowLeft;
            set => _taskFactory.StartNew(() => { _consoleImplementation.WindowLeft = value; });
        }

        public int WindowTop
        {
            get => _consoleImplementation.WindowTop;
            set => _taskFactory.StartNew(() => { _consoleImplementation.WindowTop = value; });
        }

        public int WindowWidth
        {
            get => _consoleImplementation.WindowWidth;
            set => _taskFactory.StartNew(() => { _consoleImplementation.WindowWidth = value; });
        }

        public static ISafeConsole Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>Writes the text representation of the specified 64-bit unsigned integer value to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(ulong value)
        {
            _taskFactory.StartNew(() => { _notClsConsole.Write(value); });
        }

        /// <summary>Writes the text representation of the specified 32-bit unsigned integer value to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(uint value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified array of objects to the standard output stream using the
        ///     specified format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg">An array of objects to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> or <paramref name="arg" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(string format, params object[] arg)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(format, arg); });
        }

        /// <summary>
        ///     Writes the text representation of the specified objects to the standard output stream using the specified
        ///     format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">The first object to write using <paramref name="format" />. </param>
        /// <param name="arg1">The second object to write using <paramref name="format" />. </param>
        /// <param name="arg2">The third object to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(string format, object arg0, object arg1, object arg2)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(format, arg0, arg1, arg2); });
        }

        /// <summary>
        ///     Writes the text representation of the specified objects to the standard output stream using the specified
        ///     format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">The first object to write using <paramref name="format" />. </param>
        /// <param name="arg1">The second object to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(string format, object arg0, object arg1)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(format, arg0, arg1); });
        }

        /// <summary>Writes the specified string value to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(string value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified single-precision floating-point value to the standard output
        ///     stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(float value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>Writes the text representation of the specified object to the standard output stream.</summary>
        /// <param name="value">The value to write, or null. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(object value)
        {
            _taskFactory.StartNew(() =>
            {
                _consoleImplementation.Write(value);
            });
        }

        /// <summary>
        ///     Writes the text representation of the specified object to the standard output stream using the specified
        ///     format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks). </param>
        /// <param name="arg0">An object to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(string format, object arg0)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(format, arg0); });
        }

        /// <summary>Writes the text representation of the specified 32-bit signed integer value to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(int value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified double-precision floating-point value to the standard output
        ///     stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(double value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified <see cref="T:System.Decimal" /> value to the standard output
        ///     stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(decimal value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>Writes the specified subarray of Unicode characters to the standard output stream.</summary>
        /// <param name="buffer">An array of Unicode characters. </param>
        /// <param name="index">The starting position in <paramref name="buffer" />. </param>
        /// <param name="count">The number of characters to write. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="buffer" /> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index" /> or <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="index" /> plus <paramref name="count" /> specify a position that is not within
        ///     <paramref name="buffer" />.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(char[] buffer, int index, int count)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(buffer, index, count); });
        }

        /// <summary>Writes the specified array of Unicode characters to the standard output stream.</summary>
        /// <param name="buffer">A Unicode character array. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(char[] buffer)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(buffer); });
        }

        /// <summary>Writes the specified Unicode character value to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(char value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>Writes the text representation of the specified Boolean value to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(bool value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>Writes the text representation of the specified 64-bit signed integer value to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void Write(long value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.Write(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified array of objects, followed by the current line terminator, to
        ///     the standard output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg">An array of objects to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> or <paramref name="arg" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(string format, params object[] arg)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(format, arg); });
        }

        /// <summary>
        ///     Writes the text representation of the specified objects, followed by the current line terminator, to the
        ///     standard output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">The first object to write using <paramref name="format" />. </param>
        /// <param name="arg1">The second object to write using <paramref name="format" />. </param>
        /// <param name="arg2">The third object to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(format, arg0, arg1, arg2); });
        }

        /// <summary>
        ///     Writes the text representation of the specified objects, followed by the current line terminator, to the
        ///     standard output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">The first object to write using <paramref name="format" />. </param>
        /// <param name="arg1">The second object to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(string format, object arg0, object arg1)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(format, arg0, arg1); });
        }

        /// <summary>
        ///     Writes the text representation of the specified object, followed by the current line terminator, to the
        ///     standard output stream using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">An object to write using <paramref name="format" />. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="format" /> is null.
        /// </exception>
        /// <exception cref="T:System.FormatException">The format specification in <paramref name="format" /> is invalid. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(string format, object arg0)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(format, arg0); });
        }

        /// <summary>Writes the specified string value, followed by the current line terminator, to the standard output stream.</summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(string value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified single-precision floating-point value, followed by the current
        ///     line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(float value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified object, followed by the current line terminator, to the
        ///     standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(object value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified 64-bit signed integer value, followed by the current line
        ///     terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(long value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard
        ///     output stream.
        /// </summary>
        /// <param name="buffer">An array of Unicode characters. </param>
        /// <param name="index">The starting position in <paramref name="buffer" />. </param>
        /// <param name="count">The number of characters to write. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="buffer" /> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index" /> or <paramref name="count" /> is less than zero.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="index" /> plus <paramref name="count" /> specify a position that is not within
        ///     <paramref name="buffer" />.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(char[] buffer, int index, int count)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(buffer, index, count); });
        }

        /// <summary>
        ///     Writes the text representation of the specified double-precision floating-point value, followed by the current
        ///     line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(double value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified <see cref="T:System.Decimal" /> value, followed by the current
        ///     line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(decimal value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the specified array of Unicode characters, followed by the current line terminator, to the standard
        ///     output stream.
        /// </summary>
        /// <param name="buffer">A Unicode character array. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(char[] buffer)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(buffer); });
        }

        /// <summary>
        ///     Writes the specified Unicode character, followed by the current line terminator, value to the standard output
        ///     stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(char value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified Boolean value, followed by the current line terminator, to the
        ///     standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(bool value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>Writes the current line terminator to the standard output stream.</summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine()
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(); });
        }

        /// <summary>
        ///     Writes the text representation of the specified 32-bit unsigned integer value, followed by the current line
        ///     terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(uint value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified 32-bit signed integer value, followed by the current line
        ///     terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(int value)
        {
            _taskFactory.StartNew(() => { _consoleImplementation.WriteLine(value); });
        }

        /// <summary>
        ///     Writes the text representation of the specified 64-bit unsigned integer value, followed by the current line
        ///     terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        /// <filterpriority>1</filterpriority>
        public void WriteLine(ulong value)
        {
            _taskFactory.StartNew(() => { _notClsConsole.WriteLine(value); });
        }

        public void SafeCall(Action<IConsole> action)
        {
            _taskFactory.StartNew(() =>
            {
                action(_consoleImplementation);
            });
        }
    }

    public interface ISafeConsole : IConsole, INotClsConsole
    {
        void SafeCall(Action<IConsole> action);
    }
}
