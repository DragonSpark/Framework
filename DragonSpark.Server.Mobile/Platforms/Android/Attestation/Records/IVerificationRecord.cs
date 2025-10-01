using System;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

public interface IVerificationRecord
{
    Guid Identity { get; set; }

    string KeyHash { get; set; } // The key identifier from the attestation
    DateTimeOffset Created { get; set; }
}