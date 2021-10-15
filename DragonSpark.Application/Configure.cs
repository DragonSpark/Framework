using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application;

public sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new Configure();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(DefaultRegistrations.Default)
		            .Configure(Components.Registrations.Default)
		            .ComposeUsing(Entities.Compose.Default);
}