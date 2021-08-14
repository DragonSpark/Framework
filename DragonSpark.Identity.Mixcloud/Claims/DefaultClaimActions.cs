using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Mixcloud.Claims
{
	public sealed class DefaultClaimActions : CompositeClaimAction
	{
		public static DefaultClaimActions Default { get; } = new();

		DefaultClaimActions() : base(IsProfessionalAccountAction.Default, FollowersAction.Default,
		                             CloudCastsAction.Default) {}
	}

	public class IsProfessionalAccountAction : ClaimAction
	{
		public static IsProfessionalAccountAction Default { get; } = new();

		IsProfessionalAccountAction() : base(IsProfessionalAccount.Default, "is_pro", "boolean") {}
	}

	public sealed class IsProfessionalAccount : MixcloudClaim
	{
		public static IsProfessionalAccount Default { get; } = new();

		IsProfessionalAccount() : base(nameof(IsProfessionalAccount).ToLower()) {}
	}

	public sealed class IsPremiumAccount : MixcloudClaim
	{
		public static IsPremiumAccount Default { get; } = new();

		IsPremiumAccount() : base(nameof(IsPremiumAccount).ToLower()) {}
	}

	public sealed class Followers : MixcloudClaim
	{
		public static Followers Default { get; } = new();

		Followers() : base(nameof(Followers).ToLower()) {}
	}

	public class FollowersAction : ClaimAction
	{
		public static FollowersAction Default { get; } = new();

		FollowersAction() : base(Followers.Default, "follower_count", "integer") {}
	}

	public sealed class CloudCasts : MixcloudClaim
	{
		public static CloudCasts Default { get; } = new();

		CloudCasts() : base(nameof(CloudCasts).ToLower()) {}
	}

	public class CloudCastsAction : ClaimAction
	{
		public static CloudCastsAction Default { get; } = new();

		CloudCastsAction() : base(CloudCasts.Default, "cloudcast_count", "integer") {}
	}
}