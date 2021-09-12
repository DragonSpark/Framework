using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.Eventing;
using DragonSpark.Presentation.Components.Navigation;
using DragonSpark.Presentation.Components.Routing;
using DragonSpark.Presentation.Security.Identity;
using Majorsoft.Blazor.Components.Common.JsInterop;
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
			         .Decorate<IExceptions, NavigationAwareExceptions>()
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
			         .AddScoped(typeof(IActiveContents<>), typeof(ActiveContents<>))
			         .ForDefinition<MemoryAwareActiveContents<object>>()
			         .Scoped()
			         .Then.Start<IRenderContentKey>()
			         .Forward<RenderContentKey>()
			         .Decorate<StoreAwareRenderContentKey>()
			         .Scoped()
			         //
			         .Then.Start<IApplyQueryStringValues>()
			         .Forward<ApplyQueryStringValues>()
			         .Scoped()
			         //
			         .Then.Start<ICurrentPrincipal>()
			         .Forward<CurrentPrincipal>()
			         .Singleton()
			         //
			         .Then.Start<IContentInteraction>()
			         .Forward<ContentInteraction>()
			         .Include(x => x.Dependencies)
			         .Scoped()
			         //
			         .Then.Start<DefaultExternalLogin>()
			         .And<IsPreRendering>()
			         .Include(x => x.Dependencies)
			         .Scoped()
			         //
			         .Then.AddJsInteropExtensions()
				;
		}
	}
}