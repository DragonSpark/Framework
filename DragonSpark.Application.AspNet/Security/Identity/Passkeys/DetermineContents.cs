using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class DetermineContents : IStopAware<DetermineContentsInput, ResponseResult>
{
    readonly DetermineResponse _response;
    readonly ComposeCode       _code;
    readonly ResponseResult    _error;

    public DetermineContents(ComposeCode code) : this(DetermineResponse.Default, code, ErrorResponse.Default) {}

    public DetermineContents(DetermineResponse response, ComposeCode code, ResponseResult error)
    {
        _response = response;
        _code     = code;
        _error    = error;
    }

    public async ValueTask<ResponseResult> Get(Stop<DetermineContentsInput> parameter)
    {
        var response = await _response.Off(parameter);
        var result   = !response.IsNullOrWhiteSpace() ? new(_code.Get(response)) : _error;
        return result;
    }
}