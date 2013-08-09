using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Objects.Synchronization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace DragonSpark.Web.Client
{
	public interface IAuthorizer
	{
		bool IsAuthorized( IPrincipal principal );
	}

	public class AuthenticatedAuthorizer : IAuthorizer
	{
		public bool IsAuthorized( IPrincipal principal )
		{
			var result = principal.Identity.IsAuthenticated;
			return result;
		}
	}

	public class AnonymousAuthorizer : IAuthorizer
	{
		public bool IsAuthorized( IPrincipal principal )
		{
			var result = !principal.Identity.IsAuthenticated;
			return result;
		}
	}


	public class ClaimsAuthorizer : IAuthorizer
	{
		public string Type { get; set; }

		public string Value { get; set; }

		public bool IsAuthorized( IPrincipal principal )
		{
			var result = principal.AsTo<ClaimsPrincipal, bool>( x => x.FindFirst( y => y.Type == Type ).Transform( y => Value == null || Value == y.Value ) );
			return result;
		}
	}

	public class RolesAuthorizer : IAuthorizer
	{
		public string Roles { get; set; }

		public bool IsAuthorized( IPrincipal principal )
		{
			var result = Roles.ToStringArray().All( principal.IsInRole );
			return result;
		}
	}

	public class NavigationBuilder : Factory<ShellNode, Navigation>
	{
		readonly static char Alt = Path.AltDirectorySeparatorChar;
		readonly IPrincipal principal;
		readonly string rootPath;

		public NavigationBuilder( IPrincipal principal, string rootPath = "viewmodels" )
		{
			this.principal = principal;
			this.rootPath = rootPath;
		}

		protected override Navigation CreateItem( ShellNode parameter )
		{
			var result = new Navigation
			{
				Shell = Create<ShellInfo>( parameter, rootPath ),
				NotFound = Create<RouteInfo>( parameter.NotFound, rootPath ),
				Routes = parameter.Children.Select( x => Create( x, rootPath ) ).NotNull().ToArray()
			};
			return result;
		}

		static TInfo Create<TInfo>( ClientModule node, string path ) where TInfo : RouteInfo, new()
		{
			var result = new TInfo().SynchronizeFrom( node );
			var p = result.Path ?? node.Path;
			result.Path = path.NullIfEmpty().Transform( x => string.Concat( x, Alt, p ) ) ?? p;
			result.Route = result.Route ?? path.NullIfEmpty().Transform( x => DetermineRoute( result.Path ) ) ?? result.Path;
			result.Tag = result.Tag.Transform( x => string.Concat( x.StartsWith( "#" ) ? string.Empty : "#", x ) );
			return result;
		}

		static string DetermineRoute( string path )
		{
			var queue = new Queue<string>( path.ToStringArray( Alt ) );
			queue.Dequeue();
			var result = string.Join( Alt.ToString(), queue.ToArray() );
			return result;
		}

		RouteInfo Create( NavigationNode node, string path = null )
		{
			if ( node.Authorizers.All( x => x.IsAuthorized( principal ) ) )
			{
				var result = Create<RouteInfo>( node, path );
				result.Children = node.Children.Select( x => Create( x ) ).NotNull().ToArray();
				return result;
			}
			return null;
		}
	}
}