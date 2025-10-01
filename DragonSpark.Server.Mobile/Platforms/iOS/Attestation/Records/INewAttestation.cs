using System;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public interface INewAttestation : IStopAware<NewAttestationRecordInput, Guid?>;