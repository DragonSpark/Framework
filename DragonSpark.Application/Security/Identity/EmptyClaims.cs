namespace DragonSpark.Application.Security.Identity
{
	public sealed class EmptyClaims : Claims
	{
		public static EmptyClaims Default { get; } = new EmptyClaims();

		EmptyClaims() : base(_ => false) {}
	}
}