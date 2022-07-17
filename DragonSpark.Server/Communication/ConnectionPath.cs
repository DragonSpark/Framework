using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Flurl;
using Microsoft.Extensions.Configuration;
using System;

namespace DragonSpark.Server.Communication;

public class ConnectionPath : Instance<Uri>, ISelect<string, Uri>
{
	protected ConnectionPath(IConfiguration configuration, string name)
		: this(new Uri(configuration.GetConnectionString(name))) {}

	protected ConnectionPath(Uri root) : base(root) {}

	public Uri Get(string parameter) => Get().AppendPathSegment(parameter).ToUri();
}