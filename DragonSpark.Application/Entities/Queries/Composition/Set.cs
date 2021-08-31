using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	sealed class Set<T> : Query<T> where T : class
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<T>>>(Set<T> instance)
			=> instance.Then().Remove();

		public static Set<T> Default { get; } = new Set<T>();

		Set() : base(x => x.Set<T>()) {}
	}

	public class Set<TIn, T> : Query<TIn, T> where T : class
	{
		public static Set<TIn, T> Default { get; } = new();

		Set() : base(x => x.Set<T>()) {}
	}
}