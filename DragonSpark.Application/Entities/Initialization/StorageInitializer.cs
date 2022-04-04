using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

public sealed class StorageInitializer<T> : IHostInitializer where T : DbContext
{
	readonly IMutable<IServiceProvider?> _services;
	readonly Array<IInitialize>          _initializers;

	public StorageInitializer(IEnumerable<IInitialize> initializers)
		: this(CurrentServices.Default, initializers.Open()) {}

	public StorageInitializer(IMutable<IServiceProvider?> services, params IInitialize[] initializers)
	{
		_services     = services;
		_initializers = initializers;
	}

	public async ValueTask Get(IHost parameter)
	{
		await using var context = await parameter.Services.GetRequiredService<IDbContextFactory<T>>()
		                                         .CreateDbContextAsync()
		                                         .ConfigureAwait(false);

		using var _ = _services.Assigned(parameter.Services);
		foreach (var initializer in _initializers.Open())
		{
			await initializer.Await(context);
		}
	}
}