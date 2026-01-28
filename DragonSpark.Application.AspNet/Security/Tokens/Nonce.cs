using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Tokens;

[Index(nameof(ExpiresAtUtc)), Index(nameof(Purpose), nameof(ExpiresAtUtc))]
public sealed class Nonce
{
    [Key, MaxLength(64)]
    public required string Key { get; init; }

    public DateTime IssuedAtUtc { get; init; }
    public DateTime ExpiresAtUtc { get; init; }

    [MaxLength(256)]
    public string? Scope { get; init; }

    public NoncePurpose Purpose { get; init; }
    public DateTime? UsedAtUtc { get; init; }
}