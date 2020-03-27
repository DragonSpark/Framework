using DragonSpark.Application.Entities;
using DragonSpark.Model.Selection;
using LightInject;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfiguredContexts<T> : ISelect<(IServiceFactory Factory, T Current), T> where T : DbContext
	{
		public static ConfiguredContexts<T> Default { get; } = new ConfiguredContexts<T>();

		ConfiguredContexts() {}

		public T Get((IServiceFactory Factory, T Current) parameter)
		{
			var (factory, current) = parameter;

			using var scope = factory.BeginScope();
			var result = scope.GetInstance<IStorageInitializer<T>>().Get(current);
			return result;
		}
	}
}