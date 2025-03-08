using System.Threading.Tasks;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Editing;

[MustDisposeResource]
sealed class CommitAwareEditor : IEditor
{
	readonly DatabaseFacade _facade;
	readonly IEditor        _previous;

	[MustDisposeResource(false)]
	public CommitAwareEditor(DatabaseFacade facade, IEditor previous)
	{
		_facade   = facade;
		_previous = previous;
	}

	public async ValueTask Get()
	{
		await _previous.Off();
		if (_facade.CurrentTransaction is not null)
		{
			await _facade.CurrentTransaction.CommitAsync().Off();
			await _facade.BeginTransactionAsync().Off();
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
