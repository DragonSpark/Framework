using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Security.Identity.Profile;

public record ProfileBase(string Identifier, string FirstName, string LastName, string DisplayName);

// TODO
public sealed record Profile(string Identifier, string FirstName, string LastName, string DisplayName)
	: ProfileBase(Identifier, FirstName, LastName, DisplayName);

public sealed class DefaultProfile : Instance<Profile>
{
	public static DefaultProfile Default { get; } = new();

	DefaultProfile() : base(new (string.Empty, string.Empty, string.Empty, string.Empty)) {}
}