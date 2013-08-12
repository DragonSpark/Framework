using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using DotNetOpenAuth.AspNet;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Security;
using Microsoft.Web.WebPages.OAuth;

namespace DragonSpark.Server.Security
{
	[Singleton( typeof(IAuthenticationResultProcessor) )]
	class AuthenticationResultProcessor : IAuthenticationResultProcessor
	{
		readonly IClaimsRepository repository;
		readonly IUserProfileSynchronizer synchronizer;

		public AuthenticationResultProcessor( IClaimsRepository repository, IUserProfileSynchronizer synchronizer )
		{
			this.repository = repository;
			this.synchronizer = synchronizer;
		}

		public bool Process( AuthenticationResult authenticationResult )
		{
			var result = authenticationResult.IsSuccessful;
			var module = FederatedAuthentication.SessionAuthenticationModule;
			if ( result && module != null )
			{
				Persist( authenticationResult, module );
			}
			return result;
		}

		[MethodImpl( MethodImplOptions.Synchronized )]
		void Persist( AuthenticationResult result, System.IdentityModel.Services.SessionAuthenticationModule module )
		{
			var claims = repository.DetermineClaims( result.Provider, result.ExtraData ).ToList();

			var now = DateTime.UtcNow;
			claims.AddRange( new[]
			{
				new Claim( ClaimTypes.AuthenticationInstant, now.ToString( "s" ) ),
				new Claim( Claims.IdentityProvider, OAuthWebSecurity.GetOAuthClientData( result.Provider ).Transform( x => x.DisplayName ) ?? result.Provider ),
				claims.Find( x => x.Type == ClaimTypes.NameIdentifier ) == null ? new Claim( ClaimTypes.NameIdentifier, result.ProviderUserId ) : null,
				claims.Find( x => x.Type == ClaimTypes.Upn ) == null ? new Claim( ClaimTypes.Upn, result.UserName ) : null
			}.NotNull() );
			
			var identity = new ClaimsIdentity( claims, result.Provider, System.IdentityModel.Claims.ClaimTypes.NameIdentifier, null );
			var principal = new ClaimsPrincipal( identity );
			synchronizer.Apply( principal );

			var context = result.ExtraData.ContainsKey("accesstoken") ? result.ExtraData["accesstoken"] : string.Empty;
			var validTo = now.Add( module.CookieHandler.PersistentSessionLifetime.GetValueOrDefault() );
			var token = new SessionSecurityToken( principal, context, now, validTo ) { IsPersistent = true };
			module.WriteSessionTokenToCookie( token );
		}
	}
}