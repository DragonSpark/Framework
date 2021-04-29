using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Flurl;
using Microsoft.Extensions.Configuration;
using System;

namespace DragonSpark.Server.Communication
{
	public class ConnectionPath : Instance<Uri>, ISelect<string, Uri>
	{
		public ConnectionPath(IConfiguration configuration, string name)
			: this(new Uri(configuration.GetConnectionString(name))) {}

		public ConnectionPath(Uri root) : base(root) {}

		public Uri Get(string parameter) => Get().AppendPathSegment(parameter).ToUri();
	}
}