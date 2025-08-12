using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed record AuthenticationData(
    Array<byte> Payload,
    Array<byte> RelyingPartyIdentifierHash,
    byte Flags,
    uint Count,
    AttestedCredential Credential);