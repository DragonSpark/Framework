using DragonSpark.Application.Model.Interaction;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new Configure();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(DefaultRegistrations.Default, Components.Registrations.Default,
		                       Registrations.Default)
		            .ComposeUsing(Entities.Compose.Default);
}