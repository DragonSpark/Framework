using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

sealed class CommitAwareEditor : IEditor
{
	readonly DatabaseFacade _facade;
	readonly IEditor        _previous;

	public CommitAwareEditor(DatabaseFacade facade, IEditor previous)
	{
		_facade   = facade;
		_previous = previous;
	}

	public async ValueTask Get()
	{
		await _previous.Await();
		if (_facade.CurrentTransaction is not null)
		{
			await _facade.CurrentTransaction.CommitAsync().ConfigureAwait(false);
			await _facade.BeginTransactionAsync().ConfigureAwait(false);
		}
	}

	public void Dispose()
	{
		_previous.Dispose();
	}

	public void Add(object entity)
	{
		_previous.Add(entity);
	}

	public void Attach(object entity)
	{
		_previous.Attach(entity);
	}

	public void Detach(object entity)
	{
		_previous.Detach(entity);
	}

	public void Update(object entity)
	{
		_previous.Update(entity);
	}

	public void Remove(object entity)
	{
		_previous.Remove(entity);
	}

	public void Clear()
	{
		_previous.Clear();
	}

	public ValueTask Refresh(object entity) => _previous.Refresh(entity);
}