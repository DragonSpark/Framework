using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Components.Content;

namespace DragonSpark.Presentation
{
	sealed class Configure : IAlteration<BuildHostContext>
	{
		public static Configure Default { get; } = new Configure();

		Configure() {}

		public BuildHostContext Get(BuildHostContext parameter)
			=> parameter.Configure(DefaultRegistrations.Default)
			            .Configure(Interaction.Registrations.Default)
			            .ComposeUsing(x => x.Decorate(typeof(IActiveContents<>),
			                                          typeof(PreRenderAwareActiveContents<>)));
	}
}