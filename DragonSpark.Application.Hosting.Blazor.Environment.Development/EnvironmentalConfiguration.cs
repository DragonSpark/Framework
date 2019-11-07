using DragonSpark.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Blazor.Environment
{
	public sealed class EnvironmentalConfiguration : IEnvironmentalConfiguration
	{
		public static EnvironmentalConfiguration Default { get; } = new EnvironmentalConfiguration();

		EnvironmentalConfiguration() {}

		public void Execute((IApplicationBuilder Builder, IWebHostEnvironment Environment) parameter)
		{
			var (builder, environment) = parameter;
			if (environment.IsDevelopment())
			{
				builder.UseDeveloperExceptionPage();
			}
			else
			{
				throw new InvalidOperationException("A call was made into an assembly component designed for development purposes, but IApplicationBuilder.IsDevelopment states that it is not.");
			}
		}
	}
}
