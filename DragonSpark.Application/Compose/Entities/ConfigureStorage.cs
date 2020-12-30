using DragonSpark.Application.Entities;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfigureStorage<T> : ICommand<IServiceCollection> where T : DbContext
	{
		readonly IStorageConfiguration _storage;

		public ConfigureStorage(IStorageConfiguration storage) => _storage  = storage;

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDbContext<T>(_storage.Get(parameter))
			         .Return(parameter)
			         .Start<IStorageInitializer<T>>()
			         .Forward<StorageInitializer<T>>()
			         .Singleton()
			         .Then.AddScoped<DbContext>(x => x.GetRequiredService<T>())
				;
		}
	}
}