using DragonSpark.Text;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition;

sealed class GetHostEnvironmentName : IFormatter<HostBuilderContext>
{
	public static GetHostEnvironmentName Default { get; } = new();

	GetHostEnvironmentName() {}

	public string Get(HostBuilderContext parameter) => parameter.HostingEnvironment.EnvironmentName;
}