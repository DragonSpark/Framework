using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class InitializationAssignments : ISelect<IServiceProvider, Assignments>
{
    public static InitializationAssignments Default { get; } = new();

    InitializationAssignments() : this(CurrentServices.Default, LogicalMigrationRegistry.Default) {}
    
    readonly IMutable<IServiceProvider?>       _services;
    readonly IMutable<IDataMigrationRegistry?> _registry;

    public InitializationAssignments(IMutable<IServiceProvider?> services, IMutable<IDataMigrationRegistry?> registry)
    {
        _services = services;
        _registry = registry;
    }

    public Assignments Get(IServiceProvider parameter)
    {
        var registry = _registry.Assigned(new DataMigrationRegistry(parameter));
        var services = _services.Assigned(parameter);
        return new(registry, services);
    }
}