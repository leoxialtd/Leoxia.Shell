using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using DryIoc;
using Leoxia.Abstractions.IO;
using Leoxia.Commands;
using Leoxia.ReadLine;

namespace Leoxia.Shell
{
    class Program
    {
        static int Main(string[] args)
        {
            // Parse command args
            // by default run interactively
            //      with -c execute the command
            //      with a file execute the file.
            try
            {
                var running = true;
                var container = new Container();
                var configuration = new IocConfiguration();
                configuration.Configure(container);
                var inputHandler = container.Resolve<IConsoleInputHandler>();
                var commandExecutor = container.Resolve<ICommandExecutor>();
                var consoleConfigurator = container.Resolve<IConsoleConfigurator>();
                consoleConfigurator.Configure();
                WriteCopyrightNotice(container);
                while (running)
                {
                    var rawLine = inputHandler.ReadLine();
                    var result = commandExecutor.Execute(rawLine);
                    running = !result.IsExit;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (Debugger.IsAttached)
                {
                    Console.WriteLine("[In Debugger]Press a key to exit...");
                    Console.ReadLine();
                }
            }
            return 0;
        }

        private static void WriteCopyrightNotice(Container container)
        {
            var console = container.Resolve<IConsole>();
            console.WriteLine("Leoxia [version " + Assembly.GetExecutingAssembly().GetName().Version + "]");
            console.WriteLine("(c) " + DateTime.Today.Year + " Leoxia Ltd. All rights reserved.");
            console.WriteLine();
        }
    }
}