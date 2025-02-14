using BlazorPro.BlazorSize;
using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.Content.Rendering.Sequences;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.Eventing;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.Routing;
using DragonSpark.Presentation.Connections;
using DragonSpark.Presentation.Environment;
using DragonSpark.Presentation.Environment.Browser.Document;
using DragonSpark.Presentation.Security.Identity;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
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
		         .Decorate<SpecificationAwareExceptionNotification>()
		         .Scoped()
		         //
		         .Then.Decorate<IExceptions, CompensationAwareExceptions>()
		         .Decorate<IExceptions, NotificationAwareExceptions>()
		         .Decorate<IExceptions, NavigationAwareExceptions>()
		         .Decorate<IExceptions, CommonUserInterfaceExceptionsAwareExceptions>()
		         .Decorate<IExceptionLogger, CommonUserInterfaceExceptionsAwareExceptionLogger>()
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
		         .Then.Start<Radzen.DialogService>()
		         .And<NotificationService>()
		         .And<ScrollToFirstValidationMessage>()
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
		         .Then.Start<IContentKey>()
		         .Forward<ContentKey>()
		         .Decorate<StoreAwareContentKey>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<IRenderContentKey>()
		         .Forward<RenderContentKey>()
		         .Scoped()
		         //
		         .Then.Start<IRenderState>()
		         .Forward<CurrentRenderState>()
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
		         //
		         .Then.Start<IDetermineContext>()
		         .Forward<DetermineContext>()
		         .Scoped()
		         //
		         .Then.Start<IEstablishContext>()
		         .Forward<EstablishContext>()
		         .Decorate<ApplicationAgentAwareEstablishContext>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IInitializeConnection>()
		         .Forward<InitializeConnection>()
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
		         .Singleton()
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
		         .Start<IMediaQueryService>()
		         .Forward<DragonSpark.Presentation.Components.Interaction.MediaQueryService>()
		         .Scoped()
		         //
		         .Then.AddFluentUIComponents()
			;
	}
}