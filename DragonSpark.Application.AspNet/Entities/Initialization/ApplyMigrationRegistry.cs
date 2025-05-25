using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class ApplyMigrationRegistry : IAllocatedStopAware<DbContext>
{
	public static ApplyMigrationRegistry Default { get; } = new();

	ApplyMigrationRegistry() : this(LogicalMigrationRegistry.Default) {}

	readonly IResult<IDataMigrationRegistry?> _registry;

	public ApplyMigrationRegistry(IResult<IDataMigrationRegistry?> registry) => _registry = registry;

	public Task Get(Stop<DbContext> parameter) => _registry.Get().Verify("Migration registry not found").Allocate(parameter);

	public Task Get(DbContext parameter, bool seeded, CancellationToken stop) => Get(new(parameter, stop));
}