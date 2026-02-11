using DragonSpark.Model.Selection;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class ComposeAttestationInput : ISelect<NewAttestationRecordInput, AttestationInput>
{
    readonly string _bundle, _environment;

    public ComposeAttestationInput(AttestationConfiguration settings)
        : this(settings.BundleIdentifier, settings.Environment) {}

    public ComposeAttestationInput(string bundle, string environment)
    {
        _bundle      = bundle;
        _environment = environment;
    }

    public AttestationInput Get(NewAttestationRecordInput parameter)
    {
        var (attestation, keyHash, challenge) = parameter;
        return new(attestation, challenge, _bundle, keyHash, _environment);
    }
}