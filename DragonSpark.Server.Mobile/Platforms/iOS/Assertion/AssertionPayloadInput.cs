using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

public readonly record struct AssertionPayloadInput(
    Array<byte> Source,
    Array<byte> PublicKey,
    string ClientDataHash);