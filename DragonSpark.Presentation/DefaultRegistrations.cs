using DragonSpark.Application.Runtime;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.Eventing;
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
			         .Then.Decorate<IExceptions, CompensationAwareExceptions>()
			         .Decorate<IExceptions, NotificationAwareExceptions>()
			         //
			         .Start<DialogService>()
			         .And<NotificationService>()
			         .Scoped()
			         //
			         .Then.Start<RouterSession>()
			         .Scoped()
			         //
			         .Then.Start<IEventAggregator>()
			         .Forward<EventAggregator>()
			         .Scoped()
					 //
			         .Then.AddScoped(typeof(IPublisher<>), typeof(Publisher<>))
				//
				;
		}
	}
}