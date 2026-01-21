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

	public uint Get(ProcessChangesInput<TFrom> parameter)
	{
		var (logger, size, _, destination, _, total) = parameter;
		var entities = _entities.Get(parameter);
		var result   = _save.Get(new(logger, size, destination, entities, total));
		return result;
	}
}