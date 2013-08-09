using DragonSpark.Entity;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Principal;
using System.Web;

namespace DragonSpark.Web.Client
{
	public class UserContext
	{
		public IPrincipal User { get; set; }

		public UserProfile Profile { get; set; }
	}

	public class PrincipalProvider : Factory<IPrincipal>
	{
		protected override IPrincipal CreateItem( object parameter )
		{
			var result = HttpContext.Current.User;
			return result;
		}
	}

	/*public class CurrentUserExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = HttpContext.Current.User;
			return result;
		}
	}*/

	public class UserProfileFactory<TUserProfile> : Factory<IPrincipal, TUserProfile> where TUserProfile : UserProfile
	{
		readonly DbContext context;

		public UserProfileFactory( DbContext context )
		{
			this.context = context;
		}

		protected override TUserProfile CreateItem( IPrincipal parameter )
		{
			return parameter.Identity.IsAuthenticated ? Load( parameter ) : null;
		}

		TUserProfile Load( IPrincipal parameter )
		{
			var set = context.Set<TUserProfile>();
			var id = parameter.Identity.Name;
			var result = set.Find( id ).To<TUserProfile>();
			context.Include( result, x => x.Claims );
			return result;
		}
	}

	public class ClientApplicationConfiguration
	{
		public Uri AuthenticationUri { get; set; }

		public UserProfile UserProfile { get; set; }

		public bool EnableDebugging { get; set; }

		public string Logo { get; set; }

		public ApplicationDetails ApplicationDetails { get; set; }

		public Navigation Navigation { get; set; }

		public IEnumerable<ClientModule> Initializers { get; set; }

		public IEnumerable<ClientModule> Commands { get; set; }

		public IEnumerable<WidgetModule> Widgets { get; set; }

		public ResourcesContainer Resources { get; set; }
	}
}