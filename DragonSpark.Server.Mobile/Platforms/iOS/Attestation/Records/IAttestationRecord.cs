using System;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public interface IAttestationRecord
{
    string Id { get; set; } // The key identifier from the attestation
    DateTimeOffset Created { get; set; }
    byte[] PublicKeyHash { get; set; } // SHA256 hash of the public key
    byte[] PublicKey { get; set; }     // SHA256 hash of the public key
    byte[] Receipt { get; set; }       // Receipt from the attestation statement
    uint Count { get; set; }
}

public interface IAttestationRecord<T> : IStopAware<ValidatedAttestationInput, T?>;