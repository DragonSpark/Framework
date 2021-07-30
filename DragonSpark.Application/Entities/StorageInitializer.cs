using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class StorageInitializer<T> : IHostInitializer where T : DbContext
	{
		readonly Array<IInitializer<T>> _initializers;

		public StorageInitializer(params IInitializer<T>[] initializers) => _initializers = initializers;

		public async ValueTask Get(IHost parameter)
		{
			var context = parameter.Services.GetRequiredService<T>();
			foreach (var initializer in _initializers.Open())
			{
				await initializer.Await(context);
			}
		}
	}
}