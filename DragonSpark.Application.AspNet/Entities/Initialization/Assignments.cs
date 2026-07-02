using System;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public readonly struct Assignments(Assignment<IDataMigrationRegistry> registry, Assignment<IServiceProvider> services)
    : IDisposable
{
    public void Dispose()
    {
        registry.Dispose();
        services.Dispose();
    }
}