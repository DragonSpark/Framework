using System.Security.Principal;

namespace DragonSpark.Server.Legacy.Security.Security
{
	public interface IUserProfileSynchronizer
	{
		void Apply( IPrincipal principal );
	}
}