using DragonSpark.Server.Mobile.Security.Devices.Validation;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public interface IAttestationRecord : IValidationRecord
{
    byte[] PublicKeyHash { get; set; } // SHA256 hash of the public key
    byte[] PublicKey { get; set; }     // SHA256 hash of the public key
    byte[] Receipt { get; set; }       // Receipt from the attestation statement
    uint Count { get; set; }
}