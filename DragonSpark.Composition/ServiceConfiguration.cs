using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition;

public class ServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
{
	public ServiceConfiguration(ICommand<IServiceCollection> command) : base(command) {}

	public ServiceConfiguration(Action<IServiceCollection> command) : base(command) {}
}