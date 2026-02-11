using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

sealed class ValidVerification : IValidVerification
{
    readonly IProcessIntegrityToken _token;
    readonly IFormatter<string>     _formatter;

    public ValidVerification(IProcessIntegrityToken token) : this(token, TokenFormatter.Default) {}

    public ValidVerification(IProcessIntegrityToken token, IFormatter<string> formatter)
    {
        _token     = token;
        _formatter = formatter;
    }

    public async ValueTask<bool> Get(Stop<NewAttestationRecordInput> parameter)
    {
        var ((input, _, challenge), stop)            = parameter;
        var (request, (application, _), (device, _)) = await _token.Off(new(input, stop));
        var result = application && device && _formatter.Get(request.Nonce) == challenge;
        return result;
    }
}