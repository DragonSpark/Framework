using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public sealed class ApplySeeding : ICommand<DbContextOptionsBuilder>
{
	readonly Func<DbContext, bool, CancellationToken, Task> _configure;

	public ApplySeeding(Func<Stop<DbContext>, Task> configure)
		: this((context, _, stop) => configure(context.Stop(stop))) {}

	public ApplySeeding(Func<DbContext, bool, CancellationToken, Task> configure) => _configure = configure;

	public void Execute(DbContextOptionsBuilder parameter)
	{
		parameter.UseAsyncSeeding(_configure);
	}
}