using System.Security.Principal;

namespace DragonSpark.Application.Communication.Security
{
	public interface IIdentitySynchronizer
	{
		void Apply( IPrincipal principal );
	}
}