namespace DragonSpark.Identity.Mixcloud.Claims
{
	class Class1 {}

	public sealed class DisplayName : MixcloudClaim
	{
		public static DisplayName Default { get; } = new();

		DisplayName() : base("fullname") {}
	}

	public sealed class Description : MixcloudClaim
	{
		public static Description Default { get; } = new();

		Description() : base("bio") {}
	}

	public sealed class City : MixcloudClaim
	{
		public static City Default { get; } = new();

		City() : base(nameof(City).ToLower()) {}
	}

	public sealed class ProfileUrl : MixcloudClaim
	{
		public static ProfileUrl Default { get; } = new();

		ProfileUrl() : base(nameof(ProfileUrl).ToLower()) {}
	}

	/*public sealed class ProfileImageUrl : MixcloudClaim
	{
		public static ProfileImageUrl Default { get; } = new();

		ProfileImageUrl() : base(nameof(ProfileImageUrl).ToLower()) {}
	}

	public sealed class ProfileThumbnailUrl : MixcloudClaim
	{
		public static ProfileThumbnailUrl Default { get; } = new();

		ProfileThumbnailUrl() : base(nameof(ProfileThumbnailUrl).ToLower()) {}
	}*/
}