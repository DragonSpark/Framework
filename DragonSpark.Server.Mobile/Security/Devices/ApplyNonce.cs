using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class ApplyNonce : Model.Operations.Stop.IStopAware<IssueNonceInput>
{
    readonly IIssueNonce _previous;
    readonly string      _header;

    public ApplyNonce(IIssueNonce previous) : this(previous, DpopNonceHeaderName.Default) {}

    public ApplyNonce(IIssueNonce previous, string header)
    {
        _previous = previous;
        _header   = header;
    }

    public async ValueTask Get(Stop<IssueNonceInput> parameter)
    {
        var (subject, _)                          = parameter;
        subject.Context.Response.Headers[_header] = await _previous.Off(parameter);
    }
}