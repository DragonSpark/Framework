using DragonSpark.Model.Selection;
using Flurl;
using Microsoft.Extensions.Configuration;
using System;

namespace DragonSpark.Server.Communication
{
	public class ConnectionPath : ISelect<string, Uri>
	{
		readonly Uri _root;

		public ConnectionPath(IConfiguration configuration, string name)
			: this(new Uri(configuration.GetConnectionString(name))) {}

		public ConnectionPath(Uri root) => _root = root;

		public Uri Get(string parameter) => _root.AppendPathSegment(parameter).ToUri();
	}
}