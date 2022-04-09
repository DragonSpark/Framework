using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Connections.Client;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new Configure();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter) => parameter.Configure(Registrations.Default);
}