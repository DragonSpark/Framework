using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.AspNet;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(Application.DefaultRegistrations.Default, Components.Registrations.Default,
		                       DefaultRegistrations.Default)
		            .ComposeUsing(Entities.Compose.Default);
}