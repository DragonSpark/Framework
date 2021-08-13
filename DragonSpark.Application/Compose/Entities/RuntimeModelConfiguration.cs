using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class RuntimeModelConfiguration : FixedResult<IServiceCollection, Action<DbContextOptionsBuilder>>,
	                                         IStorageConfiguration
	{
		public RuntimeModelConfiguration(IModel instance) : base(builder => builder.UseModel(instance)) {}
	}
}