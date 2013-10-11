using DragonSpark.Security;
using System;
using System.Collections.Generic;

namespace DragonSpark.Server.ClientHosting
{
	public class ClientApplicationConfiguration
	{
		public Uri AuthenticationUri { get; set; }

		public UserProfile UserProfile { get; set; }

		public bool EnableDebugging { get; set; }

		public string Logo { get; set; }

		public ApplicationDetails ApplicationDetails { get; set; }

		public Navigation Navigation { get; set; }

		public IEnumerable<ClientModule> Initializers { get; set; }

		public IEnumerable<ClientModule> Commands { get; set; }

		public IEnumerable<WidgetModule> Widgets { get; set; }

		public ServerConfiguration ServerConfiguration { get; set; }
	}
}