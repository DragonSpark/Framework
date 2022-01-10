using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Configure;

public class StorageBuilderConfiguration<T> : AppendedCommand<DbContextOptionsBuilder<T>> where T : DbContext
{
	protected StorageBuilderConfiguration(Type migrations, params object[] services)
		: base(new ConfigureSqlServer<T>(migrations), new ConfigureApplicationServices(services)) {}
}