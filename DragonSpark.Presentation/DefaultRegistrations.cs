using BlazorPro.BlazorSize;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Navigation.Security.Identity;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.Content.Sequences;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.Eventing;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.Navigation;
using DragonSpark.Presentation.Components.Routing;
using DragonSpark.Presentation.Connections.Initialization;
using DragonSpark.Presentation.Environment;
using DragonSpark.Presentation.Environment.Browser.Document;
using DragonSpark.Presentation.Security.Identity;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace DragonSpark.Presentation;

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
		         .Start<IEventAggregator>()
		         .Forward<EventAggregator>()
		         .Scoped()
		         //
		         .Then.Start<InMemoryRenderStates>()
		         .Singleton()
		         .Then.Start<RenderStates>()
		         .Use<CurrentRenderStates>()
		         .Scoped()
		         //
		         .Then.AddScoped(typeof(IPublisher<>), typeof(Publisher<>))
		         .AddScoped(typeof(IActiveContents<>), typeof(ActiveContents<>))
		         .ForDefinition<RenderAwareActiveContentBuilder<object>>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.ForDefinition<IdentifiedPagings<object>>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<ContentIdentification>()
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
		         .Then.Start<SignOut>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<ICurrentPrincipal>()
		         .Forward<CurrentPrincipal>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IContentInteraction>()
		         .Forward<ContentInteraction>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<DefaultExternalLogin>()
		         .And<IsPreRendering>()
		         .And<ContextRenderApply>()
		         .And<ClientIdentifier>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<DialogService>()
		         .And<NotificationService>()
		         .And<RouterSession>()
		         .And<NavigateToInitialize>()
		         .And<ScrollToFirstValidationMessage>()
		         .Scoped()
		         //
		         .Then.Start<ISetPageExitCheck>()
		         .Forward<SetPageExitCheck>()
		         .Decorate<ConnectionAwareSetPageExitCheck>()
		         .Scoped()
		         //
		         .Then.Start<IIsInitialized>()
		         .Forward<IsInitialized>()
		         .Singleton()
		         //
		         .Then.Start<IInitialized>()
		         .Forward<Initialized>()
		         .Decorate<ContextAwareInitialized>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<IInitializeConnection>()
		         .Forward<InitializeConnection>()
		         .Scoped()
		         //
		         .Then.Start<ResourceExistsValidation>()
		         .Singleton()
		         //
		         .Then.Start<IFocusedElement>()
		         .Forward<FocusedElement>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.AddJsInteropExtensions()
		         .AddMediaQueryService()
			;
	}
}