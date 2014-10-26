using System.Data.Entity;
using DragonSpark.IoC;
using DragonSpark.Security;

namespace DragonSpark.Application
{
	[Singleton( typeof(IUserService) )]
	public class UserService : UserService<ApplicationUserProfile>
	{
		public UserService( DbContext context ) : base( context )
		{}
	}
}