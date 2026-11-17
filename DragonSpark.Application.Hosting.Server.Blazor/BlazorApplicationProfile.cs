using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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