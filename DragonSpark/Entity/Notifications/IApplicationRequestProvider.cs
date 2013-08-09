using System.Collections.Generic;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public interface IApplicationRequestProvider
	{
		bool HasRequests();
		IEnumerable<ApplicationRequest> Retrieve( string userId );
		string Add( string userId, ApplicationRequest request );
		bool Delete( string id );
	}
}