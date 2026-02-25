using System;

namespace DragonSpark.Server.Mobile;

public readonly record struct ExistingAttestationRecordInput(
    Guid Identity,
    string Payload,
    string KeyHash,
    string Challenge);