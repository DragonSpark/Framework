using DragonSpark.Application.AspNet.Entities.Design;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public sealed class ApplySeeding : ICommand<DbContextOptionsBuilder>
{
	public static ApplySeeding Default { get; } = new();

	ApplySeeding() : this(ApplyMigrationRegistry.Default) {}

	readonly Func<DbContext, bool, CancellationToken, Task> _configure;

	public ApplySeeding(IAllocated<DbContext> configure) : this(configure.Get) {}

	public ApplySeeding(Func<Stop<DbContext>, Task> configure)
		: this((context, _, stop) => configure(context.Stop(stop))) {}

	public ApplySeeding(Func<DbContext, bool, CancellationToken, Task> configure) => _configure = configure;

	public void Execute(DbContextOptionsBuilder parameter)
	{
		parameter.UseAsyncSeeding(_configure);
	}
}