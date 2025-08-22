using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

[Index(nameof(KeyHash), IsUnique = true)]
public abstract class AttestationRecordBase : IAttestationRecord
{
    public uint Id { get; set; }

    [MaxLength(64)]
    public required string KeyHash { get; set; } // The key identifier from the attestation

    public DateTimeOffset Created { get; set; }

    public required byte[] PublicKeyHash { get; set; } // SHA256 hash of the public key
    public required byte[] PublicKey { get; set; }     // SHA256 hash of the public key
    public required byte[] Receipt { get; set; }       // Receipt from the attestation statement
    public uint Count { get; set; }
}