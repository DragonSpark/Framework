using DragonSpark.Extensions;
using DragonSpark.Objects;
using System.Security.Principal;

namespace DragonSpark.Server.ClientHosting
{
	public class PrincipalProvider : Factory<IPrincipal>
	{
		static readonly IPrincipal Anonymous = new GenericPrincipal( new GenericIdentity( string.Empty ), new string[] { } );

		protected override IPrincipal CreateItem( object parameter )
		{
			var result = ServerContext.Current.Transform( x => x.User ) ?? Anonymous;
			return result;
		}
	}
}