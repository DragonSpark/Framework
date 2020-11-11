using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Server.Environment.Development
{
	public sealed class ApplicationConfiguration : IApplicationConfiguration
	{
		[UsedImplicitly]
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() {}

		public void Execute(IApplicationBuilder parameter)
		{
			var service = parameter.ApplicationServices.GetRequiredService<IHostEnvironment>();
			if (service.IsDevelopment())
			{
				parameter.UseDeveloperExceptionPage();
			}
			else
			{
				throw new
					InvalidOperationException("A call was made into an assembly component designed for development purposes, but IApplicationBuilder.IsDevelopment states that it is not.");
			}
		}
	}
}