using DragonSpark.Compose;

namespace DragonSpark.Application.Runtime.Process;

public sealed class GetProcessIdentity : DragonSpark.Model.Results.Instance<uint>
{
    public static GetProcessIdentity Default { get; } = new();

    GetProcessIdentity() : base(System.Diagnostics.Process.GetCurrentProcess().Id.Grade()) {}
}