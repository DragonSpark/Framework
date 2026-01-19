namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ProcessChanges<TFrom, TTo> : IProcessChanges<TFrom> where TFrom : class where TTo : class
{
	readonly IComposeBatch<TFrom, TTo> _batch;
	readonly ISaveBatch<TTo>           _save;

	public ProcessChanges(IComposeBatch<TFrom, TTo> batch, ISaveBatch<TTo> save)
	{
		_batch = batch;
		_save  = save;
	}

	public uint Get(BatchInput<TFrom> parameter)
	{
		var (_, _, destination, _, _, _) = parameter;
		using var batch  = _batch.Get(parameter);
		var       result = _save.Get(new(destination, batch));
		return result;
	}
}