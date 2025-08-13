using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Mobile.Attestation;
class Class1;
public readonly record struct VerificationInput(string Challenge, string Input);

public interface IAttestation<T> : IStopAware<T>;

public interface IChallenge : IStopAware<string>;
public interface IClientKey : IStopAware<string>;

public interface IAttestationToken : IAltering<string>;

public interface IAssertionToken : IAltering<string>;

public interface IVerify<T> : IStopAware<VerificationInput, T>;