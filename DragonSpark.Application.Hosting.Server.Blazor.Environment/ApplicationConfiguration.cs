﻿using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server.Blazor.Environment
{
	sealed class ApplicationConfiguration : ICommand<IApplicationBuilder>
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() : this("/Error") {}

		readonly string _handler;

		public ApplicationConfiguration(string handler) => _handler = handler;

		public void Execute(IApplicationBuilder parameter)
		{
			parameter.UseExceptionHandler(_handler)
			         .UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		}
	}
}
