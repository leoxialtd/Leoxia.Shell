using System;
using System.Threading.Tasks;

namespace Leoxia.Commands.External
{
    public interface IProgramRunner
    {
        Task<ProgramResult> AsyncRun();
    }
}