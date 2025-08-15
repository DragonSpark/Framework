using DragonSpark.Model.Results;
using Refit;

namespace DragonSpark.Application.Communication.Http;

public sealed class DefaultSerializer : IResult<IHttpContentSerializer>
{
    public IHttpContentSerializer Get()
    {
        var options = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
        options.Converters.Remove(options.Converters[^1]);
        return new NoContentAwareSerializer(options);
    }
}