using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.AspNet;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(DefaultRegistrations.Default)
		            .Configure(DefaultRegistrationsUndo.Default)
		            .ComposeUsing(Entities.Compose.Default);
}