using DotNetOpenAuth.AspNet;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Windows.Markup;

namespace DragonSpark.Web.Configuration
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
			Clients.Apply( x => x.ClientFactory.Create<IAuthenticationClient>().NotNull( y => OAuthWebSecurity.RegisterClient( y, x.DisplayName ?? string.Empty, x.ExtraData ?? new Dictionary<string, object>() ) ) );
		}
	}

	public abstract class RegisterClientBase  : IHttpApplicationConfigurator
	{
		public string DisplayName { get; set; }

		public IDictionary<string,object> ExtraData { get; set; }


		public abstract void Configure( HttpConfiguration configuration );
	}

	public class RegisterFacebookClient : RegisterClientBase
	{
		public RegisterFacebookClient()
		{
			DisplayName = "Facebook";
		}

		public string ApplicationId { get; set; }

		public string ApplicationSecret { get; set; }

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterFacebookClient( ApplicationId, ApplicationSecret, DisplayName, ExtraData );
		}
	}

	public class RegisterYahooClient : RegisterClientBase
	{
		public RegisterYahooClient()
		{
			DisplayName = "Yahoo";
		}

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterYahooClient( DisplayName, ExtraData );
		}
	}

	public class RegisterGoogleClient : RegisterClientBase
	{
		public RegisterGoogleClient()
		{
			DisplayName = "Google";
		}

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterGoogleClient( DisplayName, ExtraData );
		}
	}

	public class RegisterMicrosoftClient : RegisterClientBase
	{
		public RegisterMicrosoftClient()
		{
			DisplayName = "Microsoft";
		}

		public string ApplicationId { get; set; }

		public string ApplicationSecret { get; set; }

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterMicrosoftClient( ApplicationId, ApplicationSecret, DisplayName, ExtraData );
		}
	}
	
	public class CustomAuthenticationClientRegistration
	{
		public IFactory ClientFactory { get; set; }

		public string DisplayName { get; set; }

		public IDictionary<string,object> ExtraData { get; set; }
	}

	[ContentProperty( "Routes" )]
	public class ApplyRoutes : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			RouteTable.Routes.With( x =>
			{
				Ignores.Apply( y => x.Ignore( y.Url, y.Constraints ) );
				Routes.Apply( y => x.MapRoute( y.RouteName ?? Guid.NewGuid().ToString(), y.Template, y.Defaults, y.Constraints ) );
				ApiRoutes.Apply( y => x.MapHttpRoute( y.RouteName ?? Guid.NewGuid().ToString(), y.Template, y.Defaults, y.Constraints ) );
			} );
		}

		public Collection<IgnoreRoute> Ignores
		{
			get { return ignores; }
		}	readonly Collection<IgnoreRoute> ignores = new Collection<IgnoreRoute>();

		public Collection<Route> Routes
		{
			get { return routes; }
		}	readonly Collection<Route> routes = new Collection<Route>();

		public Collection<Route> ApiRoutes
		{
			get { return apiRoutes; }
		}	readonly Collection<Route> apiRoutes = new Collection<Route>();
	}
}