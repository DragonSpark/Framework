using DragonSpark.Model.Results;
using Refit;
using System.Linq;

namespace DragonSpark.Application.AspNet.Communication.Http;

public sealed class ComposeRefitSettings : IResult<RefitSettings>
{
	public static ComposeRefitSettings Default { get; } = new();

	ComposeRefitSettings() {}

	public RefitSettings Get()
	{
		var options = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
		options.Converters.Remove(options.Converters.Last());
		return new (new NoContentAwareSerializer(options));
	}
}