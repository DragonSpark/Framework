using System.Collections.Generic;
using System.Data.Entity;
using DragonSpark.Application.Communication.Security;
using DragonSpark.IoC;
using DragonSpark.Features.Entity;

namespace DragonSpark.Features
{
	[Singleton( typeof(IIdentitySynchronizer) )]
	public class IdentitySynchronizer : ClaimsIdentitySynchronizer<User>
	{
		public IdentitySynchronizer( FeaturesEntityStorage context, IEnumerable<IClaimsProcessor> processors ) : base( context, processors )
		{}
	}
}