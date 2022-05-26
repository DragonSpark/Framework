using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using System;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class BlazorApplicationProfile : ApplicationProfile
{
	public static BlazorApplicationProfile Default { get; } = new BlazorApplicationProfile();

	BlazorApplicationProfile() : this(new StaticFileOptions(), EmptyCommand<IApplicationBuilder>.Default.Execute) {}

	public BlazorApplicationProfile(StaticFileOptions files, Action<IApplicationBuilder> application)
		: base(DefaultServiceConfiguration.Default.Execute,
		       Start.A.Command(application).Append(new DefaultApplicationConfiguration(files))) {}
}