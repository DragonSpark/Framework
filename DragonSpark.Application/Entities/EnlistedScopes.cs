namespace DragonSpark.Application.Entities;

sealed class EnlistedScopes : AmbientAwareScopes
{
	public EnlistedScopes(IStandardScopes previous, IAmbientContext context) : base(previous, context) {}
}