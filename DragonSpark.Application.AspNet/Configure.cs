using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter) => parameter.ComposeUsing(Entities.Compose.Default);
}