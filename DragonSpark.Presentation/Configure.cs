using DragonSpark.Application.Components;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Connections.Initialization;
using DragonSpark.Presentation.Environment;
using DragonSpark.Presentation.Security.Identity;

namespace DragonSpark.Presentation;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new Configure();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(DefaultRegistrations.Default,
		                       Environment.Browser.Registrations.Default,
		                       Environment.Browser.Time.Registrations.Default,
		                       Connections.Circuits.Registrations.Default,
		                       Diagnostics.Registrations.Default,
		                       Interaction.Registrations.Default)
		            .ComposeUsing(Registrations.Default)
		            .ComposeUsing(x => x.Decorate(typeof(IActiveContents<>), typeof(RenderingAwareActiveContents<>))
		                                .Decorate(typeof(IActiveContents<>), typeof(ExceptionAwareActiveContents<>))
		                                .Decorate<IClientIdentifier, ClientIdentifier>()
		                                .Decorate<ICurrentContext, StoreAwareCurrentContext>()
		                                //
		                                .Decorate<ISignOut, SignOut>());
}