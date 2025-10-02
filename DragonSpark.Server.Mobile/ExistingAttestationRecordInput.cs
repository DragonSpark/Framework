using System;

namespace DragonSpark.Server.Mobile;

public readonly record struct ExistingAttestationRecordInput(Guid Identity, string KeyHash );