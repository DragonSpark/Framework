using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public sealed class ProcessBearer : Variable<string>
{
    public static ProcessBearer Default { get; } = new();

    ProcessBearer() {}
}