using System.Text.Json;
using DragonSpark.Model.Results;
using Refit;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class DefaultSerializer : FixedSelection<JsonSerializerOptions, IHttpContentSerializer>
{
    public static DefaultSerializer Default { get; } = new();

    DefaultSerializer() : base(NewSerializer.Default, DefaultOptions.Default) {}
}