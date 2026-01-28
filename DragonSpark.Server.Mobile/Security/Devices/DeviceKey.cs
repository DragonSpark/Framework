using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Security.Devices;

[Index(nameof(IsBlocked)), Index(nameof(LastSeenAtUtc))]
public sealed class DeviceKey
{
    [Key, MaxLength(64)]
    public required string Identity { get; init; } // RFC7638 JWK thumbprint (base64url)

    [MaxLength(8)]
    public string Kty { get; init; } = "EC";

    [MaxLength(16)]
    public string Crv { get; init; } = "P-256";

    [MaxLength(128)]
    public string X { get; init; } = default!;

    [MaxLength(128)]
    public string Y { get; init; } = default!;

    public bool IsBlocked { get; init; }

    public required DateTime CreatedAtUtc { get; init; }

    public DateTime? AttestedAtUtc { get; init; }
    public DateTime? LastSeenAtUtc { get; init; }

    [MaxLength(32)]
    public string? EvaluationType { get; init; }

    [Timestamp]
    public byte[]? RowVersion { get; init; }
}