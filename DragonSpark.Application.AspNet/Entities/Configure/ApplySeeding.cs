using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public sealed class ApplySeeding : ICommand<DbContextOptionsBuilder>
{
	readonly Func<DbContext, bool, CancellationToken, Task> _configure;

	public ApplySeeding(Func<DbContext, Task> configure) : this((context, _, _) => configure(context)) {}

	public ApplySeeding(Func<DbContext, bool, CancellationToken, Task> configure) => _configure = configure;

	public void Execute(DbContextOptionsBuilder parameter)
	{
		parameter.UseAsyncSeeding(_configure);
	}
}