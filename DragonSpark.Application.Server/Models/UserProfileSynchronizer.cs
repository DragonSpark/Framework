using DragonSpark.IoC;
using DragonSpark.Security;
using System.Collections.Generic;

namespace DragonSpark.Application.Server.Models
{
	[Singleton( typeof(IUserProfileSynchronizer) )]
	public class UserProfileSynchronizer : UserProfileSynchronizer<ApplicationUserProfile>
	{
		public UserProfileSynchronizer( IUserService userService, IEnumerable<IClaimsProcessor> processors ) : base( userService, processors )
		{}
	}
}