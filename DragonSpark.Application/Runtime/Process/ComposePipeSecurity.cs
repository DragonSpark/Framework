using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime.Process;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
sealed class ComposePipeSecurity : IResult<PipeSecurity>
{
    public static ComposePipeSecurity Default { get; } = new();

    ComposePipeSecurity() {}

    public PipeSecurity Get()
    {
        var result    = new PipeSecurity();
        var reference = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        var rule      = new PipeAccessRule(reference, PipeAccessRights.ReadWrite, AccessControlType.Allow);
        result.AddAccessRule(rule);
        return result;
    }
}