using DragonSpark.Model.Sequences.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	public readonly struct ManyEdit<T> : IEditor
	{
		readonly IEditor _context;

		public static implicit operator Memory<T>(ManyEdit<T> instance) => instance.Subject;

		public ManyEdit(IEditor context, Leasing<T> subject)
		{
			Subject  = subject;
			_context = context;
		}

		public Leasing<T> Subject { get; }

		public void Dispose()
		{
			_context.Dispose();
			Subject.Dispose();
		}

		public ValueTask Get() => _context.Get();

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
	}
}