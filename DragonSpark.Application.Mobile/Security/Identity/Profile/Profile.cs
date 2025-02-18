namespace DragonSpark.Application.Mobile.Security.Identity.Profile;

public sealed record Profile(string Identifier, string FirstName, string LastName, string DisplayName)
	: ProfileBase(Identifier, FirstName, LastName, DisplayName);