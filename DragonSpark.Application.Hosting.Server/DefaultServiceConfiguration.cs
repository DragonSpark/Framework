using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server
{
	sealed class DefaultServiceConfiguration : ServiceConfiguration
	{
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration() : base(x => x.AddControllers()) {}
	}
}