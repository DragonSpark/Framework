namespace DragonSpark.Application.Security.Identity.Profile;

public sealed record Profile(
    string Identifier,
    string UserName,
    string ContactAddress,
    string FirstName,
    string LastName,
    string DisplayName)
    : ProfileBase(Identifier, UserName, ContactAddress, FirstName, LastName, DisplayName);