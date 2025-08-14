using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public interface IVerify<T> : IStopAware<VerificationInput, T>;