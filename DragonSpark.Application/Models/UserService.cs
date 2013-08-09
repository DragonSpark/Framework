using System.Data.Entity;
using DragonSpark.IoC;
using DragonSpark.Security;

namespace DragonSpark.Application.Models
{
	[Singleton( typeof(IUserService) )]
	public class UserService : UserService<ApplicationUserProfile>
	{
		public UserService( DbContext context ) : base( context )
		{}
	}
}