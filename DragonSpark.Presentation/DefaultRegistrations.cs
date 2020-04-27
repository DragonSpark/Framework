using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace DragonSpark.Presentation
{
	sealed class DefaultRegistrations : ICommand<IServiceCollection>
	{
		public static DefaultRegistrations Default { get; } = new DefaultRegistrations();

		DefaultRegistrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.For<IExceptions>()
			         .Map<Exceptions>()
			         .WithDependencies.Scoped()
			         //
			         .For<DialogService>()
			         .Register.Scoped();
		}
	}
}