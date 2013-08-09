using DotNetOpenAuth.AspNet.Clients;
using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Web.Security
{
	public class FacebookClient : OAuth2Client
	{
		readonly string applicationId;
		readonly string applicationSecret;
		readonly string applicationPermissions;
		readonly Facebook.FacebookClient client = new Facebook.FacebookClient();
		
		public FacebookClient( string applicationId, string applicationSecret, string applicationPermissions ) : base( "Facebook" )
		{
			this.applicationId = applicationId;
			this.applicationSecret = applicationSecret;
			this.applicationPermissions = applicationPermissions;
		}

		protected override string QueryAccessToken( Uri returnUrl, string authorizationCode )
		{
			var parameters = new { client_id = applicationId, client_secret = applicationSecret, redirect_uri = returnUrl.ToString(), code = authorizationCode };
			dynamic response = client.Get( "oauth/access_token", parameters );
			var result = response.access_token;
			return result;
		}

		protected override Uri GetServiceLoginUrl( Uri returnUrl )
		{
			var parameters = new { client_id = applicationId, redirect_uri = returnUrl, scope = applicationPermissions };
			var result = client.GetLoginUrl( parameters );
			return result;
		}

		protected override IDictionary<string, string> GetUserData( string accessToken )
		{
			var result = client.Get<JsonObject>( "me", new { access_token = accessToken } ).ToDictionary( x => x.Key, x => x.Value.ToString() );
			return result;
		}
	}
}