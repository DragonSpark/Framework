using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Activation;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Configure;

public class ConfigureApplicationServices : ICommand<DbContextOptionsBuilder>
{
	readonly IServiceProvider _provider;

	public ConfigureApplicationServices(params object[] services) : this(new ServiceProvider(services)) {}

	public ConfigureApplicationServices(IServiceProvider provider) => _provider = provider;

	public void Execute(DbContextOptionsBuilder parameter)
	{
		parameter.UseApplicationServiceProvider(_provider);
	}
}