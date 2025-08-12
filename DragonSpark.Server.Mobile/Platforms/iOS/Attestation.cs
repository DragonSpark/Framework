namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed record Attestation(string Format, AttestationStatement Statement, AuthenticationData AuthenticationData);