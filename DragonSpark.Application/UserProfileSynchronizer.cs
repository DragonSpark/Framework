using System.Collections.Generic;
using DragonSpark.IoC;
using DragonSpark.Security;

namespace DragonSpark.Application
{
	[Singleton( typeof(IUserProfileSynchronizer) )]
	public class UserProfileSynchronizer : UserProfileSynchronizer<ApplicationUserProfile>
	{
		public UserProfileSynchronizer( IUserService userService, IEnumerable<IClaimsProcessor> processors ) : base( userService, processors )
		{}
	}
}