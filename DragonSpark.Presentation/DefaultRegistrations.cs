using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace DragonSpark.Presentation
{
	public sealed class Configure : IAlteration<BuildHostContext>
	{
		public static Configure Default { get; } = new Configure();

		Configure() {}

		public BuildHostContext Get(BuildHostContext parameter)
			=> parameter.Configure(DefaultRegistrations.Default);
	}

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