using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Components.Routing;
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
			parameter.Start<IExceptionNotification>()
			         .Forward<ExceptionNotification>()
			         .Decorate<ClientExceptionAwareExceptionNotification>()
			         .Scoped()
			         //
			         .Then.Start<IExceptions>()
			         .Forward<Exceptions>()
			         .Decorate<NotificationAwareExceptions>()
			         .Include(x => x.Dependencies)
			         .Scoped()
			         //
			         .Then.Start<DialogService>()
			         .Scoped()
			         //
			         .Then.Start<RouterSession>()
			         .Scoped();
		}
	}
}