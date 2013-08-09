using DragonSpark.IoC;
using DragonSpark.Security;
using System.Data.Entity;

namespace DragonSpark.Application.Server.Models
{
	[Singleton( typeof(IUserService) )]
	public class UserService : UserService<ApplicationUserProfile>
	{
		public UserService( DbContext context ) : base( context )
		{}
	}
}