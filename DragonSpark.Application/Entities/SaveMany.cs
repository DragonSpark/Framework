﻿using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities
{
	public class SaveMany<T> : Modify<T, Memory<object>> where T : DbContext
	{
		protected SaveMany(ISaveContext<T> save) : base(save, UpdateMany<object>.Default) {}
	}

	public class SaveMany<TContext, T> : Modify<TContext, Memory<T>> where TContext : DbContext where T : class
	{
		protected SaveMany(ISaveContext<TContext> save) : base(save, UpdateMany<T>.Default) {}
	}

	sealed class UpdateMany : IModification<Memory<object>>
	{
		public static UpdateMany Default { get; } = new();

		UpdateMany() {}

		public void Execute(Modify<Memory<object>> parameter)
		{
			var (context, memory) = parameter;
			for (var i = 0; i < memory.Length; i++)
			{
				context.Update(memory.Span[i]);
			}
		}
	}

	sealed class UpdateMany<T> : IModification<Memory<T>> where T : class
	{
		public static UpdateMany<T> Default { get; } = new UpdateMany<T>();

		UpdateMany() {}

		public void Execute(Modify<Memory<T>> parameter)
		{
			var (context, memory) = parameter;
			var set = context.Set<T>();
			for (var i = 0; i < memory.Length; i++)
			{
				set.Update(memory.Span[i]);
			}
		}
	}
}