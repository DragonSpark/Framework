using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Syncfusion
{
	sealed class ApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() {}

		public void Execute(IApplicationBuilder parameter)
		{
			parameter.ApplicationServices.GetRequiredService<Initializer>()
			         .Execute();
		}
	}
}