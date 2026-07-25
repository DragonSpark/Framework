using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public sealed class EmptyStorageConfiguration : IStorageConfiguration
{
	public static EmptyStorageConfiguration Default { get; } = new();

	EmptyStorageConfiguration() {}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter) => _ => {};
}