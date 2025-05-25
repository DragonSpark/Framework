using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class DataMigrationRegistry : IDataMigrationRegistry
{
	readonly IServiceProvider _services;
	readonly HashSet<ISeed>   _initializers;

	public DataMigrationRegistry(IServiceProvider services) : this(services, new()) {}

	public DataMigrationRegistry(IServiceProvider services, HashSet<ISeed> initializers)
	{
		_services     = services;
		_initializers = initializers;
	}

	public void Execute(ISeed parameter)
	{
		_initializers.Add(parameter);
	}

	public async ValueTask Get(Stop<DbContext> parameter)
	{
		foreach (var initializer in _initializers)
		{
			await initializer.Off(new(new(_services, parameter), parameter));
		}
	}
}