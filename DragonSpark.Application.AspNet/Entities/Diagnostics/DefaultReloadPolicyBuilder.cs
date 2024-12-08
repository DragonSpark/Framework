namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class DefaultReloadPolicyBuilder : ReloadPolicyBuilder
{
	public static DefaultReloadPolicyBuilder Default { get; } = new();

	DefaultReloadPolicyBuilder() : base(ReloadEntities.Default.Get) {}
}