namespace DragonSpark.Application.Entities
{
	sealed class EnlistedScopes : AmbientAwareScopes, IEnlistedScopes
	{
		public EnlistedScopes(IStandardScopes previous) : base(previous) {}
	}
}