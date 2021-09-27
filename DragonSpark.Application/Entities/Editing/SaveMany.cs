using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Editing
{
	public class SaveMany<T> : ContextualModify<T, Memory<object>> where T : DbContext
	{
		protected SaveMany(IContexts<T> contexts) : base(contexts, UpdateMany<object>.Default.Then().Operation()) {}
	}

	public class SaveMany<TContext, T> : ContextualModify<TContext, Memory<T>> where TContext : DbContext where T : class
	{
		protected SaveMany(IContexts<TContext> contexts) : base(contexts, UpdateMany<T>.Default.Then().Operation()) {}
	}

	sealed class UpdateMany : IModify<Memory<object>>
	{
		public static UpdateMany Default { get; } = new();

		UpdateMany() {}

		public void Execute(Edit<Memory<object>> parameter)
		{
			var (context, memory) = parameter;
			for (var i = 0; i < memory.Length; i++)
			{
				context.Update(memory.Span[i]);
			}
		}
	}

	sealed class UpdateMany<T> : IModify<Memory<T>> where T : class
	{
		public static UpdateMany<T> Default { get; } = new UpdateMany<T>();

		UpdateMany() {}

		public void Execute(Edit<Memory<T>> parameter)
		{
			var (context, memory) = parameter;
			for (var i = 0; i < memory.Length; i++)
			{
				context.Update(memory.Span[i]);
			}
		}
	}
}