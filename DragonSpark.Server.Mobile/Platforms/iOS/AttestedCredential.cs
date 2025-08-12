using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed record AttestedCredential(Array<byte> Identifier, Credential Credential);