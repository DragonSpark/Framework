using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class AmbientFormValueHandler : DelegatingHandler
{
    readonly IStopAware<HttpRequestMessage> _previous;

    public AmbientFormValueHandler(IAmbientProperties properties)
        : this(new ApplyAmbientFormValues(new ParseForm(properties))) {}

    public AmbientFormValueHandler(IStopAware<HttpRequestMessage> previous)
        => _previous = previous;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await _previous.Off(new(request, cancellationToken));

        return await base.SendAsync(request, cancellationToken).Off();
    }
}