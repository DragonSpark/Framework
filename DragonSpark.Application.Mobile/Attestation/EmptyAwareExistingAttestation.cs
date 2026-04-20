using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Attestation;

public sealed class EmptyAwareExistingAttestation : IExistingAttestation
{
    public static EmptyAwareExistingAttestation Default { get; } = new();

    EmptyAwareExistingAttestation() {}

    public ValueTask<ExistingAttestationResult?> Get(CancellationToken parameter)
        => default(ExistingAttestationResult?).ToOperation();
}