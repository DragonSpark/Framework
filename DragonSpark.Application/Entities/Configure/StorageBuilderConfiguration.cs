using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Configure;

public class StorageBuilderConfiguration<T> : Commands<DbContextOptionsBuilder<T>> where T : DbContext
{
	protected StorageBuilderConfiguration(Type migrations, Action<DbContextOptionsBuilder<T>> other,
	                                      params object[] services)
		: base(new UseSqlServer<T>(migrations), new ConfigureApplicationServices(services),
		       Start.A.Command(other).Get()) {}
}