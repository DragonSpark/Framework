using DragonSpark.Model.Commands;
using DragonSpark.Server.Requests.Warmup;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class DefaultApplicationConfiguration : ICommand<IApplicationBuilder>
{
	public static DefaultApplicationConfiguration Default { get; } = new();

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.UseWarmupAwareHttpsRedirection()
		         .UseStaticFiles()
		         .UseRouting()
		         .UseAuthentication()
				 .UseCors()
		         .UseAuthorization()
		         .UseAntiforgery();
	}
}