using System;
using System.Threading.Tasks;
using DeviceCheck;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class GenerateKey : IResulting<string>
{
    public static GenerateKey Default { get; } = new();

    GenerateKey() : this(DCAppAttestService.SharedService, EncodedText.Default) {}

    readonly DCAppAttestService  _service;
    readonly IAlteration<string> _text;

    public GenerateKey(DCAppAttestService service, IAlteration<string> text)
    {
        _service = service;
        _text    = text;
    }

    public async ValueTask<string> Get()
    {
        var off = _service.Supported ? await _service.GenerateKeyAsync().Off() : _text.Get(Guid.NewGuid().ToString());
        return off;
    }
}