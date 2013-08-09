using System;
using System.Linq;
using System.Net;

namespace DragonSpark.Extensions
{
	public static class CredentialHelper
	{
		public static NetworkCredential Create( string userName, string password )
		{
			var items = userName.Split( new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries );
			var name = items.LastOrDefault();
			var domain = items.Count() == 1 ? null : items.First();
			var result = new NetworkCredential( name, password, domain );
			return result;
		}
	}
}


