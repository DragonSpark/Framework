namespace DragonSpark.Application.Testing.Objects
{
	sealed class Claims : Application.Security.Identity.Claims
	{
		public static Claims Default { get; } = new Claims();

		Claims() : base(x => x.Type.StartsWith(ClaimNamespace.Default)) {}
	}
}
