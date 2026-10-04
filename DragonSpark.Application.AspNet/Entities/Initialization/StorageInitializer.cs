using DragonSpark.Compose;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class StorageInitializer<T> : IHostInitializer where T : DbContext
{
	readonly Initialize _initialize;

	public StorageInitializer(Initialize initialize) => _initialize = initialize;

	public async Task Get(IHost parameter)
	{
		var services = parameter.Services;
		await using var context = await services.GetRequiredService<IDbContextFactory<T>>()
		                                        .CreateDbContextAsync()
		                                        .Off();
		var stop = services.GetService<IHttpContextAccessor>()?.HttpContext?.RequestAborted ??
		           services.GetService<IHostApplicationLifetime>()?.ApplicationStopping ?? CancellationToken.None;

		await _initialize.Off(new(new(services, context), stop));
	}
}