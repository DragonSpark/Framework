using System.Collections.Generic;
using System.Web.Http;
using System.Windows.Markup;
using DotNetOpenAuth.AspNet;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Web.WebPages.OAuth;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Clients" )]
	public class RegisterCustomAuthenticationClients : IHttpApplicationConfigurator
	{
		public List<CustomAuthenticationClientRegistration> Clients
		{
			get { return clients; }
		}	readonly List<CustomAuthenticationClientRegistration> clients = new List<CustomAuthenticationClientRegistration>();

		public void Configure( HttpConfiguration configuration )
		{
			Clients.Apply( x => FactoryExtensions.Create<IAuthenticationClient>( x.ClientFactory ).NotNull( y => OAuthWebSecurity.RegisterClient( y, x.DisplayName ?? string.Empty, x.ExtraData ?? new Dictionary<string, object>() ) ) );
		}
	}
}