using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.Profile;

public sealed class DefaultProfile : Instance<Profile>
{
    public static DefaultProfile Default { get; } = new();

    DefaultProfile() : base(new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)) {}
}