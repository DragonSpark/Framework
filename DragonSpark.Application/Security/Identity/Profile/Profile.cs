namespace DragonSpark.Application.Security.Identity.Profile;

public sealed record Profile(
    string Identifier,
    string UserName,
    string Address,
    string FirstName,
    string LastName,
    string DisplayName)
    : ProfileBase(Identifier, UserName, Address, FirstName, LastName, DisplayName);