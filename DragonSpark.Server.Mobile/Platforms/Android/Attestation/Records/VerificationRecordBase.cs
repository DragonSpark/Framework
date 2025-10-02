using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

[Index(nameof(KeyHash), nameof(Identity), IsUnique = true)]
public abstract class VerificationRecordBase : IVerificationRecord
{
    public uint Id { get; set; }

    [MaxLength(64)]
    public required string KeyHash { get; set; }

    public DateTimeOffset Created { get; set; }

    public Guid Identity { get; set; }
}