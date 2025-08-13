using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed record Attestation(string Format, AttestationStatement Statement, AuthenticationData AuthenticationData);

// TODO

public interface IAttestationRecord
{
    string Id { get; set; } // The key identifier from the attestation
    DateTimeOffset Created { get; set; }
    string PublicKeyHash { get; set; } // SHA256 hash of the public key
    byte[] PublicKey { get; set; }     // SHA256 hash of the public key
    byte[] Receipt { get; set; }       // Receipt from the attestation statement
    ushort Count { get; set; }
}

public abstract class AttestationRecordBase : IAttestationRecord
{
    public required string Id { get; set; }         // The key identifier from the attestation
    public DateTimeOffset Created { get; set; }
    
    [MaxLength(32)]
    public required string PublicKeyHash { get; set; } // SHA256 hash of the public key
    public required byte[] PublicKey { get; set; } // SHA256 hash of the public key
    public byte[] Receipt { get; set; } = [];      // Receipt from the attestation statement
    public ushort Count { get; set; }
}
