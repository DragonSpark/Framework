using System.Text.Json;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime.Objects;

public sealed class FrameworkSerializerOptions : Instance<JsonSerializerOptions>
{
    public static FrameworkSerializerOptions Default { get; } = new();

    FrameworkSerializerOptions() : base(new(JsonSerializerDefaults.Web)) {}
}