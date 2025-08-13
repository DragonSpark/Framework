using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

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
        var (attestation, challenge, bundleId, keyHash, environment, format) = parameter;

        var instance = _parser.Get(attestation);
        return instance is not null
                   ? _valid.Get(new(instance, challenge, bundleId, keyHash, environment, format)) ? instance : null
                   : null;
    }
}