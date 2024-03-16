using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Security;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.Content.Rendering.Sequences;
using DragonSpark.Presentation.Environment;
using Registrations = DragonSpark.Presentation.Components.Content.Sequences.Virtualization.Registrations;

namespace DragonSpark.Presentation;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new ();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(DefaultRegistrations.Default,
		                       Environment.Browser.Registrations.Default,
		                       Environment.Browser.Time.Registrations.Default,
		                       Connections.Circuits.Registrations.Default,
		                       Components.Forms.Registrations.Default,
		                       Registrations.Default)
		            .ComposeUsing(Composing.Default)
		            .ComposeUsing(x => x.Decorate(typeof(IActiveContents<>), typeof(RenderingAwareActiveContents<>))
		                                .Decorate(typeof(IActiveContents<>), typeof(ExceptionAwareActiveContents<>))
		                                .Decorate(typeof(IPaging<>), typeof(RenderAwarePaging<>))
		                                .Decorate<ICurrentContext, StoreAwareCurrentContext>());
}