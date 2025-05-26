using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public class EntityRangeInitializer<T> : IInitializer where T : class
{
	readonly Array<T> _entities;

	protected EntityRangeInitializer(params T[] entities) => _entities = entities;

	public async ValueTask Get(DragonSpark.Model.Operations.Stop<DbContext> parameter)
	{
		var (subject, stop) = parameter;
		subject.Set<T>().AddRange(_entities);
		await subject.SaveChangesAsync(stop).Off();
	}
}