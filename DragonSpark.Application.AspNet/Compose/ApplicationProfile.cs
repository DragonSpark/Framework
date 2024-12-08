using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.AspNet.Compose;

public class ApplicationProfile : Application.Compose.Undo.ApplicationProfile, IApplicationProfile
{
	readonly Action<IApplicationBuilder> _application;

	public ApplicationProfile(ICommand<IServiceCollection> services, ICommand<IApplicationBuilder> application)
		: this(services.Execute, application.Execute) {}

	public ApplicationProfile(Action<IServiceCollection> services, Action<IApplicationBuilder> application)
		: base(services) => _application = application;

	public void Execute(IApplicationBuilder parameter)
	{
		_application(parameter);
	}
}