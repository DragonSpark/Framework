using AspNet.Security.OAuth.DeviantArt;

namespace DragonSpark.Identity.DeviantArt
{
	public sealed class DeviantArtApplicationSettings
	{
		public string Key { get; set; } = null!;

		public string Secret { get; set; } = null!;

		public string UserInformationEndpoint { get; set; } = $"{DeviantArtAuthenticationDefaults.UserInformationEndpoint}?expand=user.profile";
	}
}