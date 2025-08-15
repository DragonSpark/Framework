using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public abstract class AttestationRecordBase : IAttestationRecord
{
    [MaxLength(32)]
    public required string Id { get; set; } = null!; // The key identifier from the attestation

    public DateTimeOffset Created { get; set; }

    public required byte[] PublicKeyHash { get; set; } = null!; // SHA256 hash of the public key
    public required byte[] PublicKey { get; set; } = null!;     // SHA256 hash of the public key
    public byte[] Receipt { get; set; } = [];                   // Receipt from the attestation statement
    public uint Count { get; set; }
}