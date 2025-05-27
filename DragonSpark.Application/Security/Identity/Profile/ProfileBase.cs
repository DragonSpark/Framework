namespace DragonSpark.Application.Security.Identity.Profile;

public abstract record ProfileBase(
    string Identifier,
    string UserName,
    string Address,
    string FirstName,
    string LastName,
    string DisplayName);