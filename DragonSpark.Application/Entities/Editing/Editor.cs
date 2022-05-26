using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

sealed class Editor : IEditor
{
	readonly DbContext   _context;
	readonly IDisposable _disposable;

	public Editor(DbContext context) : this(context, context) {}

	public Editor(DbContext context, IDisposable disposable)
	{
		_context    = context;
		_disposable = disposable;
	}

	public async ValueTask Get()
	{
		await _context.SaveChangesAsync().ConfigureAwait(false);
	}

	public void Add(object entity)
	{
		_context.Add(entity);
	}

	public void Attach(object entity)
	{
		_context.Attach(entity);
	}

	public void Update(object entity)
	{
		_context.Update(entity);
	}

	public void Remove(object entity)
	{
		var record = _context.Entry(entity);
		switch (record.State)
		{
			case EntityState.Added:
			case EntityState.Modified:
			case EntityState.Unchanged:
				_context.Remove(entity);
				break;
		}
	}

	public void Clear()
	{
		_context.ChangeTracker.Clear();
	}

	public ValueTask Refresh(object entity) => _context.Entry(entity).ReloadAsync().ToOperation();

	public void Dispose()
	{
		_disposable.Dispose();
	}
}