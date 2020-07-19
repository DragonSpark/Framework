using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Model
{
	public static class Empty
	{
		public static Array<T> Result<T>() => Empty<T>.Array;
		
		public static IEnumerable<T> Enumerable<T>() => Empty<T>.Enumerable;

		public static T[] Array<T>() => Empty<T>.Array;

		public static IQueryable<T> Queryable<T>() => Empty<T>.Array.AsQueryable();

	}

	public static class Empty<T>
	{
		public static T[] Array { get; } = System.Array.Empty<T>();

		public static ArraySegment<T> Segment { get; } = new ArraySegment<T>(Array);

		public static IEnumerable<T> Enumerable { get; } = System.Linq.Enumerable.Empty<T>();

		public static ImmutableArray<T> Immutable { get; } = ImmutableArray<T>.Empty;
	}
}