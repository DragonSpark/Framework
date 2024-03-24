using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using System;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class BlazorApplicationProfile : ApplicationProfile
{
	public static BlazorApplicationProfile Default { get; } = new();

	BlazorApplicationProfile() : this(EmptyCommand<IApplicationBuilder>.Default.Execute) {}

	public BlazorApplicationProfile(Action<IApplicationBuilder> configure)
		: base(DefaultServiceConfiguration.Default.Execute,
		       Start.A.Command(configure).Append(DefaultApplicationConfiguration.Default)) {}
}

sealed class BlazorApplicationProfile<T> : ApplicationProfile
{
	public static BlazorApplicationProfile<T> Default { get; } = new();

	BlazorApplicationProfile()
		: this(EmptyCommand<IApplicationBuilder>.Default.Execute) {}

	public BlazorApplicationProfile(Action<IApplicationBuilder> configure)
		: this(configure, ApplyBlazorWebApplication<T>.Default.Execute) {}

	public BlazorApplicationProfile(Action<IApplicationBuilder> configure, Action<IApplicationBuilder> post)
		: base(DefaultServiceConfiguration.Default.Execute,
		       Start.A.Command(configure).Append(DefaultApplicationConfiguration.Default).Append(post)) {}
}