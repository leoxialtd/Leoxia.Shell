﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.Infrastructure;
using Leoxia.Commands.Threading;

namespace Leoxia.Commands.External
{
    public interface IProgramRunnerFactory
    {
        IProgramRunner CreateRunner(string command);
    }

    public class ProgramRunnerFactory : IProgramRunnerFactory
    {
        private readonly IExecutableResolver _resolver;
        private readonly ISafeConsole _safeConsole;
        private readonly IDirectory _directory;

        public ProgramRunnerFactory(IExecutableResolver resolver, 
            ISafeConsole safeConsole, 
            IDirectory directory)
        {
            _resolver = resolver;
            _safeConsole = safeConsole;
            _directory = directory;
        }


        public IProgramRunner CreateRunner(string command)
        {
            return new ProgramRunner(_resolver, _safeConsole, _directory, command);
        }
    }
}