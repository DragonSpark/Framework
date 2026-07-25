using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class DefaultExpiration : Instance<TimeSpan>
{
    public static DefaultExpiration Default { get; } = new();

    DefaultExpiration() : base(TimeSpan.FromMinutes(2)) {}
}