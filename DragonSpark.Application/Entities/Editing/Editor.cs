using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
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
			_context.Remove(entity);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}