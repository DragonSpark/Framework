namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

public sealed record DeviceRecord(
    string DeviceId,
    string Kty,
    string Crv,
    string X,
    string Y,
    bool IsBlocked,
    DateTime? AttestedAtUtc,
    DateTime? LastSeenAtUtc,
    string? EvaluationType);