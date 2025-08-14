using System.Threading.Tasks;
using DeviceCheck;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class GenerateKey : IResulting<string>
{
    public static GenerateKey Default { get; } = new();

    GenerateKey() : this(DCAppAttestService.SharedService) {}

    readonly DCAppAttestService _service;

    public GenerateKey(DCAppAttestService service) => _service = service;

    public ValueTask<string> Get() => _service.GenerateKeyAsync().ToOperation();
}