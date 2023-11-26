﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

public class MigrateToTarget : IInitialize
{
	readonly IResulting<string?> _name;
	readonly IInitialize         _latest;

	protected MigrateToTarget(IResulting<string?> name) : this(name, Migrate.Default) {}

	protected MigrateToTarget(IResulting<string?> name, IInitialize latest)
	{
		_name   = name;
		_latest = latest;
	}

	public async ValueTask Get(DbContext parameter)
	{
		var name = await _name.Await();
		if (name is not null)
		{
			await parameter.GetInfrastructure()
			               .GetService<IMigrator>()
			               .Verify()
			               .MigrateAsync(name)
			               .ConfigureAwait(false);
		}
		else
		{
			await _latest.Await(parameter);
		}
	}
}