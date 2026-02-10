using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Tokens;

[Index(nameof(ExpiresAtUtc)), Index("Discriminator", nameof(ExpiresAtUtc))]
public abstract class Nonce
{
    [Key, MaxLength(64)]
    public required string Key { get; set; }

    public DateTime IssuedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }

    [MaxLength(256)]
    public string? Scope { get; set; }

    public DateTime? UsedAtUtc { get; set; }
}