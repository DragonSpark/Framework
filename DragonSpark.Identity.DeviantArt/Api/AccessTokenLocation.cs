using System;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class AccessTokenLocation : Model.Results.Instance<Uri>
{
	public AccessTokenLocation(DeviantArtApplicationSettings settings)
		: base(new
			       Uri($"https://www.deviantart.com/oauth2/token?grant_type=client_credentials&client_id={settings.Key}&client_secret={settings.Secret}")) {}
}