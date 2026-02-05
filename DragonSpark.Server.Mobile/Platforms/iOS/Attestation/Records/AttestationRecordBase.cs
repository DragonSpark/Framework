using DragonSpark.Server.Mobile.Security.Devices.Validation;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public abstract class AttestationRecordBase : ValidationRecordBase, IAttestationRecord
{
    public required byte[] PublicKeyHash { get; set; } // SHA256 hash of the public key
    public required byte[] PublicKey { get; set; }     // SHA256 hash of the public key
    public required byte[] Receipt { get; set; }       // Receipt from the attestation statement
    public uint Count { get; set; }
}