using System.Text.Json;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class DefaultOptions : FixedSelection<JsonNamingPolicy?, JsonSerializerOptions>
{
    public static DefaultOptions Default { get; } = new();

    DefaultOptions() : base(NewOptions.Default, null) {}
}