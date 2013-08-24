using System.Security.Principal;
using DragonSpark.Security;

namespace DragonSpark.Server.ClientHosting
{
	public class UserContext
	{
		public IPrincipal User { get; set; }

		public UserProfile Profile { get; set; }
	}
}