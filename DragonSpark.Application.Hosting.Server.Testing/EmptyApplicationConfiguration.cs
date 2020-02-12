using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server.Testing
{
	public sealed class EmptyApplicationConfiguration : IApplicationConfiguration
	{
		[UsedImplicitly]
		public static EmptyApplicationConfiguration Default { get; } = new EmptyApplicationConfiguration();

		EmptyApplicationConfiguration() {}

		public void Execute(IApplicationBuilder parameter) {}
	}
}