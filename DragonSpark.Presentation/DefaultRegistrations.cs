using BlazorPro.BlazorSize;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Navigation.Security.Identity;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.Content.Rendering.Sequences;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.Eventing;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.Navigation;
using DragonSpark.Presentation.Components.Routing;
using DragonSpark.Presentation.Connections;
using DragonSpark.Presentation.Environment;
using DragonSpark.Presentation.Environment.Browser.Document;
using DragonSpark.Presentation.Security.Identity;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace DragonSpark.Presentation;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new();

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
		         .Decorate<ILogException, NavigationAwareLogException>()
		         //
		         .Start<IEventAggregator>()
		         .Forward<EventAggregator>()
		         .Scoped()
		         //
		         .Then.Start<RenderStateMonitor>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<DefaultExternalLogin>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<DialogService>()
		         .And<NotificationService>()
		         .And<RouterSession>()
		         .And<ScrollToFirstValidationMessage>()
		         .And<ContentIdentification>()
		         .And<ResourceExistsValidation>()
		         .Scoped()
		         //
		         .Then.AddScoped(typeof(IPublisher<>), typeof(Publisher<>))
		         .AddScoped(typeof(IActiveContents<>), typeof(ActiveContents<>))
		         //
		         .ForDefinition<RenderingAwareActiveContents<object>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.ForDefinition<RenderStateAwarePagingContents<object>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         .Then.ForDefinition<RenderStateAwareAnyContents<object>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IRenderContentKey>()
		         .Forward<RenderContentKey>()
		         .Decorate<StoreAwareRenderContentKey>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IApplyQueryStringValues>()
		         .Forward<ApplyQueryStringValues>()
		         .Scoped()
		         //
		         .Then.Start<ICurrentPrincipal>()
		         .Forward<CurrentPrincipal>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<ISetPageExitCheck>()
		         .Forward<SetPageExitCheck>()
		         .Decorate<ConnectionAwareSetPageExitCheck>()
		         .Scoped()
		         .Then.Start<IInitializeConnection>()
		         .Forward<InitializeConnection>()
		         .Decorate<ContextAwareInitializeConnection>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<IConnectionIdentifier>()
		         .Forward<ConnectionIdentifier>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<IResourceQuery>()
		         .Forward<ResourceQuery>()
		         .Decorate<StoreAwareResourceQuery>()
		         .Scoped()
		         //
		         .Then.Start<IFocusedElement>()
		         .Forward<FocusedElement>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<RenderCache>()
		         .Singleton()
		         //
		         .Then.AddJsInteropExtensions()
		         .AddMediaQueryService()
			;
	}
}