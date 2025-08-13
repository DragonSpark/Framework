using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed record CredentialIdentifier(Array<byte> Value, ushort Length);