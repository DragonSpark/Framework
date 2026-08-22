using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class BlazorApplicationProfile : ApplicationProfile
{
	public static BlazorApplicationProfile Default { get; } = new();

	BlazorApplicationProfile() : this(DefaultServiceConfiguration.Default.Execute, _ => {}) {}

	public BlazorApplicationProfile(Action<IApplicationBuilder> configure) : this(32, configure) {}

	public BlazorApplicationProfile(byte receive, Action<IApplicationBuilder> configure)
		: this(new DefaultServiceConfiguration(receive).Execute, configure) {}

	public BlazorApplicationProfile(Action<IServiceCollection> services, Action<IApplicationBuilder> configure)
		: base(services, Start.A.Command(configure).Append(DefaultApplicationConfiguration.Default)) {}
}

sealed class BlazorApplicationProfile<T> : ApplicationProfile
{
	public static BlazorApplicationProfile<T> Default { get; } = new();

	BlazorApplicationProfile() : this(_ => {}) {}

	public BlazorApplicationProfile(Action<IApplicationBuilder> configure, params Assembly[] additional)
		: this(DefaultServiceConfiguration.Default.Execute, configure, additional) {}

	public BlazorApplicationProfile(Action<IServiceCollection> services, Action<IApplicationBuilder> configure,
	                                params Assembly[] additional)
		: this(services, configure, new ApplyBlazorWebApplication<T>(additional).Execute) {}

	public BlazorApplicationProfile(Action<IServiceCollection> services, Action<IApplicationBuilder> configure,
	                                Action<IApplicationBuilder> post)
		: base(services, Start.A.Command(configure).Append(DefaultApplicationConfiguration.Default).Append(post)) {}
}