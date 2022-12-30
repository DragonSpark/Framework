using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

public class EntityRangeInitializer<T> : IInitializer where T : class
{
	readonly Array<T> _entities;

	protected EntityRangeInitializer(params T[] entities) => _entities = entities;

	public async ValueTask Get(DbContext parameter)
	{
		parameter.Set<T>().AddRange(_entities);
		await parameter.SaveChangesAsync().ConfigureAwait(false);
	}
}