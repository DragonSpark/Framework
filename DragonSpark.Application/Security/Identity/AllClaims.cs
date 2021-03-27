namespace DragonSpark.Application.Security.Identity
{
	public sealed class AllClaims : Claims
	{
		public static AllClaims Default { get; } = new AllClaims();

		AllClaims() : base(_ => true) {}
	}
}