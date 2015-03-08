using System.Linq;
using System.Net;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application
{
	partial class ClientExceptionDetail
	{
		partial void OnCreate()
		{
			var address = ServiceLocation.With<IApplicationParameterSource,IPAddress>( x => x.Retrieve<ClientExceptionDetail, string>( "IpAddress" ).Transform( IPAddress.Parse ) );
			IpAddresses = address.ToString().ToEnumerable().ToArray();
		}
	}
}
