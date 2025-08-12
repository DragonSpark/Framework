using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed record CredentialIdentifier(Array<byte> Value, ushort Length);