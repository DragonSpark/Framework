namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed record Attestation(string Format, AttestationStatement Statement, AuthenticationData AuthenticationData);

