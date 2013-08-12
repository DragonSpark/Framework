using System.Security.Principal;
using System.Web;
using DragonSpark.Objects;

namespace DragonSpark.Server.Client
{
	public class PrincipalProvider : Factory<IPrincipal>
	{
		protected override IPrincipal CreateItem( object parameter )
		{
			var result = HttpContext.Current.User;
			return result;
		}
	}
}