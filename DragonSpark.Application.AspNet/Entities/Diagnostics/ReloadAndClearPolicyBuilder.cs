using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Diagnostics;

sealed class ReloadAndClearPolicyBuilder : ReloadPolicyBuilder
{
	public static ReloadAndClearPolicyBuilder Default { get; } = new();

	ReloadAndClearPolicyBuilder() : base(ReloadEntities.Default.Then().Append(ClearEntityStores.Default)) {}
}