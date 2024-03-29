﻿using DragonSpark.Composition;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Testing.Environment;

public sealed class RootServiceConfiguration : IServiceConfiguration
{
	[UsedImplicitly]
	public static RootServiceConfiguration Default { get; } = new();

	RootServiceConfiguration() {}

	public void Execute(IServiceCollection parameter) {}
}