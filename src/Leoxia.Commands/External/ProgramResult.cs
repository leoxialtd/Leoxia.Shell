namespace Leoxia.Commands.External
{
    public class ProgramResult
    {
        public ProgramResult(string output, string error, int exitCode)
        {
            Output = output;
            Error = error;
            ExitCode = exitCode;
        }

        public string Output { get; }

        public string Error { get; }

        public int ExitCode { get; }

        public static implicit operator string(ProgramResult result)
        {
            return result.Output;
        }
    }
}