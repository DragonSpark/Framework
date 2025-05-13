using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Communication;

public class ConnectionPath : Instance<Uri>, ISelect<string, Uri>
{
    protected ConnectionPath(IConfiguration configuration, string name)
        : this(new(configuration.GetConnectionString(name)
                                .Verify($"The configuration section {name} was not found"))) {}

    protected ConnectionPath(Uri root) : base(root) {}

    public Uri Get(string parameter) => new(Get(), parameter);
}