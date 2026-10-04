using DragonSpark.Application.AspNet.Entities.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class InitializeMigration<T> : IStopAware where T : DbContext
{
	readonly INewContext<T>   _context;
	readonly Initialize       _initializer;
	readonly IServiceProvider _services;

	public InitializeMigration(INewContext<T> context, Initialize initializer, IServiceProvider services)
	{
		_context     = context;
		_initializer = initializer;
		_services    = services;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		await using var context = _context.Get();
		await _initializer.Off(new(new(_services, context), parameter));
	}
}