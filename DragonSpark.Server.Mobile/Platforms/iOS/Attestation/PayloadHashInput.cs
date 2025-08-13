using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public readonly record struct PayloadHashInput(Array<byte> Payload, string Challenge);