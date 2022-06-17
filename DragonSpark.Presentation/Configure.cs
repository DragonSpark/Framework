using DragonSpark.Application.Security;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Environment;

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
		            .ComposeUsing(x => x/*.Decorate(typeof(IActiveContents<>), typeof(RenderingAwareActiveContents<>)) TODO */
		                                .Decorate(typeof(IActiveContents<>), typeof(ExceptionAwareActiveContents<>))
		                                .Decorate<ICurrentContext, StoreAwareCurrentContext>());
}