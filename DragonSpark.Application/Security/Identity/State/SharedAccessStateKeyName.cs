using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity.State;

sealed class SharedAccessStateKeyName : Instance<string>
{
	public static SharedAccessStateKeyName Default { get; } = new();

	SharedAccessStateKeyName() : base("application-shared-state") {}
}