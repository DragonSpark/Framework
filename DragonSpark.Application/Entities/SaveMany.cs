using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class SaveMany<T> : IOperation<Memory<object>> where T : DbContext
	{
		readonly IContexts<T> _context;

		public SaveMany(IContexts<T> context) => _context = context;

		public async ValueTask Get(Memory<object> parameter)
		{
			await using var context  = _context.Get();
			for (var i = 0; i < parameter.Length; i++)
			{
				context.Update(parameter.Span[i]);
			}
			await context.SaveChangesAsync();
		}
	}

	public class SaveMany<TContext, T> : IOperation<Memory<T>> where TContext : DbContext where T : class
	{
		readonly IContexts<TContext> _context;

		public SaveMany(IContexts<TContext> context) => _context = context;

		public async ValueTask Get(Memory<T> parameter)
		{
			await using var context = _context.Get();
			var             set     = context.Set<T>();
			for (var i = 0; i < parameter.Length; i++)
			{
				set.Update(parameter.Span[i]);
			}
			await context.SaveChangesAsync();
		}
	}
}