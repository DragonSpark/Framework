using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;

namespace DragonSpark.Syncfusion
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Register<SyncfusionConfiguration>()
			         .AddSyncfusionBlazor()
			         .Start<Initializer>()
			         .Singleton();
		}
	}
}