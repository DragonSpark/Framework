using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class ApplyBlazorApplication : Commands<IApplicationBuilder>
{
	public ApplyBlazorApplication(Action<IApplicationBuilder> start, ICommand<IApplicationBuilder> complete)
		: this(start, complete.Execute) {}

	public ApplyBlazorApplication(Action<IApplicationBuilder> start, Action<IApplicationBuilder> complete)
		: this(start, DefaultApplicationConfiguration.Default.Execute, complete) {}

	public ApplyBlazorApplication(Action<IApplicationBuilder> start, Action<IApplicationBuilder> command,
	                              Action<IApplicationBuilder> complete)
		: base(start, command, complete) {}
}