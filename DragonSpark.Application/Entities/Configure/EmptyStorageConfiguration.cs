using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

public sealed class EmptyStorageConfiguration : IStorageConfiguration
{
	public static EmptyStorageConfiguration Default { get; } = new EmptyStorageConfiguration();

	EmptyStorageConfiguration() {}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter) => _ => {};
}