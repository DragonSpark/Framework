namespace DragonSpark.Application.Security.Identity.Profile;

public abstract record ProfileBase(
    string Identifier,
    string UserName,
    string ContactAddress,
    string FirstName,
    string LastName,
    string DisplayName);