using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

sealed class EnvironmentalStorageConfiguration
	: SelectedInstanceSelector<IServiceCollection, Action<DbContextOptionsBuilder>>,
	  IStorageConfiguration
{
	public static EnvironmentalStorageConfiguration Default { get; } = new EnvironmentalStorageConfiguration();

	EnvironmentalStorageConfiguration()
		: base(Start.An.Instance(DefaultServiceComponentLocator<IStorageConfiguration>.Default)
		            .Then()
		            .Select(x => x.ToDelegate())) {}
}