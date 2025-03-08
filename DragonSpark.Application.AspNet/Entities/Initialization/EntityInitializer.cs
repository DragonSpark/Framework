using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public class EntityInitializer<T> : IInitializer where T : class
{
	readonly Array<T> _entities;

	protected EntityInitializer(params T[] entities) => _entities = entities;

	public async ValueTask Get(DbContext parameter)
	{
		var set = parameter.Set<T>();
		foreach (var entity in _entities.Open())
		{
			set.Add(entity);
			await parameter.SaveChangesAsync().Off();
		}
	}
}