using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using System;
using System.Reflection;

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

	public BlazorApplicationProfile(Action<IApplicationBuilder> configure, params Assembly[] additional)
		: this(configure, new ApplyBlazorWebApplication<T>(additional).Execute) {}

	public BlazorApplicationProfile(Action<IApplicationBuilder> configure, Action<IApplicationBuilder> post)
		: base(DefaultServiceConfiguration.Default.Execute,
		       Start.A.Command(configure).Append(DefaultApplicationConfiguration.Default).Append(post)) {}
}