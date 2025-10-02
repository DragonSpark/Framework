using System;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public interface IExistingAttestation : IStopAware<ExistingAttestationRecordInput, Guid?>;