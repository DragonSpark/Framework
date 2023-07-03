using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class WebsiteClaimAction : CustomClaimAction
{
	public static WebsiteClaimAction Default { get; } = new();

	WebsiteClaimAction() : base(Website.Default, "data", GetUrl.Instance.Get!) {}

	sealed class GetUrl : ISelect<JsonElement, string?>
	{
		public static GetUrl Instance { get; } = new();

		GetUrl() {}

		public string? Get(JsonElement parameter)
		{
			var root     = parameter.GetProperty("data");
			var entities = root.GetProperty("entities");
			if (entities.TryGetProperty("url", out entities) && entities.TryGetProperty("urls", out var urls))
			{
				return urls[0].GetString("expanded_url");
			}
			return null;
		}
	}
}