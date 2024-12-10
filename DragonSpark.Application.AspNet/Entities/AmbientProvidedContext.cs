using System;
using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities;

sealed class AmbientProvidedContext : IResult<DbContext?>
{
	public static AmbientProvidedContext Default { get; } = new();

	AmbientProvidedContext() : this(AmbientProvider.Default) {}

	readonly IResult<IServiceProvider?> _provider;

	public AmbientProvidedContext(IResult<IServiceProvider?> provider) => _provider = provider;

	[MustDisposeResource(false)]
	public DbContext? Get() => _provider.Get()?.GetRequiredService<DbContext>();
}
