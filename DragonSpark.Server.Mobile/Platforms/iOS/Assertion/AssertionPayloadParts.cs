using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

public readonly record struct AssertionPayloadParts(
    Array<byte> NonceHash,
    Array<byte> Signature,
    Array<byte> Authentication);