using JetBrains.Annotations;

namespace DragonSpark.Application.Testing.Objects {
	sealed class DisplayNameClaim : Claim
	{
		[UsedImplicitly]
		public static DisplayNameClaim Default { get; } = new DisplayNameClaim();

		DisplayNameClaim() : base("displayName") {}
	}
}