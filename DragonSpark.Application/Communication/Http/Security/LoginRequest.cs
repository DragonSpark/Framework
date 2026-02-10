namespace DragonSpark.Application.Communication.Http.Security;

public sealed record LoginRequest(string Address, string Identifier) : Contracts.Security.LoginRequest(Address);