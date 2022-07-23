using DragonSpark.Application.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Connections.Client;

sealed class Configure : IAlteration<ApplicationProfileContext>
{
	public static Configure Default { get; } = new Configure();

	Configure() {}

	public ApplicationProfileContext Get(ApplicationProfileContext parameter)
		=> parameter.Append(Registrations.Default);
}