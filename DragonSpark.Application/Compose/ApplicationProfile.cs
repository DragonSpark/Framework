using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose
{
	public class ApplicationProfile : IApplicationProfile
	{
		readonly Action<IApplicationBuilder> _application;
		readonly Action<IServiceCollection>  _services;

		public ApplicationProfile(Action<IServiceCollection> services, Action<IApplicationBuilder> application)
		{
			_services    = services;
			_application = application;
		}

		public void Execute(IServiceCollection parameter)
		{
			_services(parameter);
		}

		public void Execute(IApplicationBuilder parameter)
		{
			_application(parameter);
		}
	}
}