using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Selection;
using System.Text.Json;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class FollowersClaimAction : CustomClaimAction
{
	public static FollowersClaimAction Default { get; } = new();

	FollowersClaimAction() : base(Followers.Default, "data", GetUrl.Instance.Get!) {}

	sealed class GetUrl : ISelect<JsonElement, string?>
	{
		public static GetUrl Instance { get; } = new();

		GetUrl() {}

		public string? Get(JsonElement parameter)
		{
			if (parameter.TryGetProperty("data", out var data) &&
			    data.TryGetProperty("public_metrics", out var metrics) &&
			    metrics.TryGetProperty("following_count", out var count) && count.TryGetInt64(out var result))
			{
				return result.ToString();
			}

			return null;
		}
	}
}