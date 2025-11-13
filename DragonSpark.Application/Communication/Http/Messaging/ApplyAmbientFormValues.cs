using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class ApplyAmbientFormValues : IStopAware<HttpRequestMessage>
{
    readonly ICondition<HttpRequestMessage> _condition;
    readonly IParser<FormUrlEncodedContent> _parser;

    public ApplyAmbientFormValues(IParser<FormUrlEncodedContent> parser) : this(IsFormContent.Default, parser) {}

    public ApplyAmbientFormValues(ICondition<HttpRequestMessage> condition, IParser<FormUrlEncodedContent> parser)
    {
        _condition = condition;
        _parser    = parser;
    }

    public async ValueTask Get(Stop<HttpRequestMessage> parameter)
    {
        var (subject, stop) = parameter;

        if (_condition.Get(parameter))
        {
            var content = subject.Content is not null
                              ? await subject.Content.ReadAsStringAsync(stop).Off()
                              : string.Empty;
            subject.Content = _parser.Get(content);
        }
    }
}