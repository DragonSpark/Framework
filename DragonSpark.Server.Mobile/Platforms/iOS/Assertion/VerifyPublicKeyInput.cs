using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

public readonly record struct VerifyPublicKeyInput(Array<byte> Hash, Array<byte> Key);