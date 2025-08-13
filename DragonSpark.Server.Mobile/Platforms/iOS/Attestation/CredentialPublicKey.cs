using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed record CredentialPublicKey(Array<byte> Value, Array<byte> Hash);