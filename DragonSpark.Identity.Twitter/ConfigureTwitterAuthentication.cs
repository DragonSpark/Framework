using DragonSpark.Identity.Twitter.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Twitter;
using System;

namespace DragonSpark.Identity.Twitter
{
	sealed class ConfigureTwitterAuthentication : ICommand<TwitterOptions>
	{
		readonly Func<TwitterApplicationSettings> _settings;

		public ConfigureTwitterAuthentication(Func<TwitterApplicationSettings> settings) => _settings = settings;

		public void Execute(TwitterOptions parameter)
		{
			var settings = _settings();
			parameter.ConsumerKey         = settings.Key;
			parameter.ConsumerSecret      = settings.Secret;
			parameter.RetrieveUserDetails = true;
			parameter.ClaimActions.MapJsonKey(DisplayName.Default, "name", "string");
			parameter.ClaimActions.MapJsonKey(Description.Default, "description", "string");
			parameter.ClaimActions.MapCustomJson(Website.Default, "url", root => root.GetProperty("entities")
			                                                                         .GetProperty("url")
			                                                                         .GetProperty("urls")[0]
			                                                                         .GetString("expanded_url"));
			parameter.ClaimActions.MapJsonKey(ImagePath.Default, "profile_image_url_https", "url");
		}
	}
}