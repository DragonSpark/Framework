using System.Net;

namespace DragonSpark.Server
{
	public static class Network
	{
		public static IPAddress Determine( string address )
		{
			var data = address == "::1" ? "127.0.0.1" : address;
			var result = IPAddress.Parse( data );
			return result;
		}
	}
}