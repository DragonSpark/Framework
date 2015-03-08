using System.Linq;
using System.Net;

namespace DragonSpark.Application
{
	partial class ClientExceptionDetail
	{
		partial void OnCreate()
		{
			MachineName = System.Environment.MachineName;
			IpAddresses = Dns.GetHostAddresses( Dns.GetHostName() ).Select( x => x.ToString() ).ToArray();
		}
	}
}
