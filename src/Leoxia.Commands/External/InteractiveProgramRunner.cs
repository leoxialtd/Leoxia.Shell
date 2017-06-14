using System;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.Threading;

namespace Leoxia.Commands.External
{
    public sealed class InteractiveProgramRunner : BaseProgramRunner
    {
        public InteractiveProgramRunner(
            IExecutableResolver resolver,
            ISafeConsole safeConsole,
            IDirectory directory,
            string commandLine) : base(resolver, safeConsole, directory, commandLine)
        {
        }

        protected override void BeforeStart()
        {            
        }

        protected override void AfterStart()
        {
        }

        protected override void OnExit()
        {
        }

        protected override ProgramResult GetResult(int exitCode)
        {
            return new ProgramResult(String.Empty, string.Empty, exitCode);
        }
    }
}