using System.Security.Principal;

namespace DragonSpark.Security
{
	public interface IUserProfileSynchronizer
	{
		void Apply( IPrincipal principal );
	}
}