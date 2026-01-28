using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ProcessChanges<TFrom, TTo> : IProcessChanges<TFrom> where TFrom : class where TTo : class
{
	readonly IEntities<TFrom, TTo> _entities;
	readonly ISave<TTo>            _save;

	public ProcessChanges(IEntities<TFrom, TTo> entities, ISave<TTo> save)
	{
		_entities = entities;
		_save     = save;
	}

	public ValueTask<uint> Get(Stop<ProcessChangesInput<TFrom>> parameter)
	{
		var ((logger, size, _, destination, _, total), stop) = parameter;
		var entities = _entities.Get(parameter);
		var result   = _save.Get(new(new(logger, size, destination, entities, total), stop));
		return result;
	}
}