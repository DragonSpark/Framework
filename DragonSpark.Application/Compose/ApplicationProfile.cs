using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose;

public class ApplicationProfile : Command<IServiceCollection>, IApplicationProfile
{
	public ApplicationProfile(ICommand<IServiceCollection> services) : base(services) {}

	public ApplicationProfile(Action<IServiceCollection> services) : base(services) {}
}