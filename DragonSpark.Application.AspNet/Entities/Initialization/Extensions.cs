﻿using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public static class Extensions
{
	public static IServiceCollection WithAutomaticMigrations(this IServiceCollection @this)
		=> Initialization.WithAutomaticMigrations.Default.Parameter(@this);
}