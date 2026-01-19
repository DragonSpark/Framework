using System.Text.Json;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class FrameworkSerializerOptions : Instance<JsonSerializerOptions>
{
    public static FrameworkSerializerOptions Default { get; } = new();

    FrameworkSerializerOptions() : base(new(JsonSerializerDefaults.Web)) {}
}