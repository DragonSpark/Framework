using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Editing;

[MustDisposeResource]
sealed class Editor : DragonSpark.Model.Operations.Allocated.Terminating<int>, IEditor
{
	readonly DbContext   _context;
	readonly IDisposable _disposable;

	/*public Editor(DbContext context) : this(context, context) {}*/

	public Editor(DbContext context, IDisposable disposable) : base(context.Save)
	{
		_context    = context;
		_disposable = disposable;
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
			case EntityState.Detached:
				_context.Attach(entity);
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
