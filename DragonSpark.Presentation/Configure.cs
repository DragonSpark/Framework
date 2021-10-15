using DragonSpark.Application.Components;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Connections.Initialization;

namespace DragonSpark.Presentation;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new Configure();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(DefaultRegistrations.Default)
		            .Configure(Interaction.Registrations.Default)
		            .ComposeUsing(x => x.Decorate(typeof(IActiveContents<>),
		                                          typeof(PreRenderAwareActiveContents<>))
		                                .Decorate<IClientIdentifier, ClientIdentifier>());
}