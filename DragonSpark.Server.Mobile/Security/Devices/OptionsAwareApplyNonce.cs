using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Options;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class OptionsAwareApplyNonce : Model.Operations.Stop.IStopAware<IssueNonceInput>
{
    readonly IOptions<DevicePoPOptions> _options;
    readonly ApplyNonce                 _previous;

    public OptionsAwareApplyNonce(IOptions<DevicePoPOptions> options, ApplyNonce previous)
    {
        _options  = options;
        _previous = previous;
    }

    public ValueTask Get(Stop<IssueNonceInput> parameter)
        => _options.Value.RequireNonce ? _previous.Get(parameter) : ValueTask.CompletedTask;
}