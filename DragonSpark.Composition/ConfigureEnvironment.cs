using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Activation;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition
{
	sealed class ConfigureEnvironment : IAlteration<IHostBuilder>, IActivateUsing<string>
	{
		public static ISelect<string, IAlteration<IHostBuilder>> Defaults { get; }
			= Start.A.Selection<string>()
			       .By.StoredActivation<ConfigureEnvironment>()
			       .Stores()
			       .Reference();

		readonly string _environment;

		public ConfigureEnvironment(string environment) => _environment = environment;

		public IHostBuilder Get(IHostBuilder parameter) => parameter.UseEnvironment(_environment);
	}
}