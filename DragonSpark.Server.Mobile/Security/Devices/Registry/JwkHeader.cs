namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

public sealed record JwkHeader(string Kty, string Crv, string X, string Y);