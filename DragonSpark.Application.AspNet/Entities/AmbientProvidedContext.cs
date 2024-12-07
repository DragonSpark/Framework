using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities;

sealed class AmbientProvidedContext : IResult<DbContext?>
{
	public static AmbientProvidedContext Default { get; } = new();

	AmbientProvidedContext() : this(AmbientProvider.Default) {}

	readonly IResult<IServiceProvider?> _provider;

	public AmbientProvidedContext(IResult<IServiceProvider?> provider) => _provider = provider;

	public DbContext? Get() => _provider.Get()?.GetRequiredService<DbContext>();
}