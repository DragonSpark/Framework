using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class DesignTimeIdentityOptions : Instance<IdentityOptions>, IOptions<IdentityOptions>
{
    public static DesignTimeIdentityOptions Default { get; } = new();

    DesignTimeIdentityOptions() : base(new() { Stores = { SchemaVersion = IdentitySchemaVersions.Version3 } }) {}

    public IdentityOptions Value => Get();
}