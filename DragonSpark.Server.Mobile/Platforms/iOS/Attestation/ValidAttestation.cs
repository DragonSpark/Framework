using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed class ValidAttestation : AllCondition<AttestationInstanceInput>, IValidAttestation
{
    public static ValidAttestation Default { get; } = new();

    ValidAttestation()
        : base(ValidAttestationKey.Default, ValidAttestationBundle.Default, ValidAttestationFormat.Default,
               ValidAttestationEnvironment.Default, ValidAttestationChallenge.Default) {}
}