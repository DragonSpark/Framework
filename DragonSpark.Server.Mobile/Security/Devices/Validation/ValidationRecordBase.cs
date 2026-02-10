using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

[Index(nameof(Thumbprint), IsUnique = true)]
[Index(nameof(KeyHash), nameof(Identity), IsUnique = true)]
public abstract class ValidationRecordBase : IValidationRecord
{
    public uint Id { get; set; }

    [MaxLength(64)]
    public required string KeyHash { get; set; }

    public DateTimeOffset Created { get; set; }

    public Guid Identity { get; set; }

    [MaxLength(43)]
    public required string Thumbprint { get; set; }
}