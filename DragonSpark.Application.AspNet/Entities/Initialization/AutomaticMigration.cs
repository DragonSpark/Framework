using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class AutomaticMigration : IInitialize
{
	readonly AutomaticMigrationSettings _settings;
	readonly IInitialize                _migrate;

	public AutomaticMigration(AutomaticMigrationSettings settings) : this(settings, Migrate.Default) {}

	public AutomaticMigration(AutomaticMigrationSettings settings, Migrate migrate)
	{
		_settings = settings;
		_migrate  = migrate;
	}

	public ValueTask Get(DbContext parameter)
	{
		if (_settings.Enabled)
		{
			var target = _settings.Target;
			return target is not null
				       ? parameter.GetInfrastructure()
				                  .GetService<IMigrator>()
				                  .Verify()
				                  .MigrateAsync(target)
				                  .ToOperation()
				       : _migrate.Get(parameter);
		}

		return ValueTask.CompletedTask;
	}
}