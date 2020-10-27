using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public class StorageConfiguration : Select<IServiceCollection, Action<DbContextOptionsBuilder>>,
	                                    IStorageConfiguration
	{
		public StorageConfiguration(Action<DbContextOptionsBuilder> configure)
			: this(configure.Start().Accept<IServiceCollection>()) {}

		public StorageConfiguration(Func<IServiceCollection, Action<DbContextOptionsBuilder>> select) : base(select) {}
	}
}