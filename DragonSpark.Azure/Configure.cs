using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Azure;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(Registrations.Default)
		            .Configure(Messaging.Messages.Queues.Registrations.Default)
		            .Configure(Ai.Registrations.Default);
}