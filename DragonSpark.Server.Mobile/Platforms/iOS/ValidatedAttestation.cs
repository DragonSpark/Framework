using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed class ValidatedAttestation : ISelect<ValidatedAttestationInput, Attestation?>
{
    public static ValidatedAttestation Default { get; } = new();

    ValidatedAttestation() : this(AttestationParser.Default, ValidAttestation.Default) {}

    readonly IParser<Attestation?>             _parser;
    readonly ICondition<ValidAttestationInput> _valid;

    public ValidatedAttestation(IParser<Attestation?> parser, ICondition<ValidAttestationInput> valid)
    {
        _parser = parser;
        _valid  = valid;
    }

    public Attestation? Get(ValidatedAttestationInput parameter)
    {
        var (challenge, bundleId, keyHash, attestation, format) = parameter;

        var instance = _parser.Get(attestation);
        if (instance is not null)
        {
            // TODO Additional checks: app ID, environment, etc.
            // Store receipt/public key hash for future assertions#1#

            var valid = _valid.Get(new(instance, challenge, bundleId, keyHash, format));
            return valid ? instance : null;
        }

        return null;
    }
}