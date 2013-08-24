using System;
using System.Collections.Generic;
using DragonSpark.Security;

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

		public ResourcesContainer Resources { get; set; }
	}
}