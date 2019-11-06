using System;
using System.Linq.Expressions;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Objects;
using DragonSpark.Text;
using DragonSpark.Text.Formatting;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static Pair<Type, Func<string, Func<object, IProjection>>> Entry<T>(this IFormattedProjection<T> @this)
			=> Pairs.Create(A.Type<T>(),
			                @this.Select(Compose.Start.A.Selection.Of.Any.AndOf<T>().By.Cast.Or.Throw.Select)
			                     .Select(x => x.ToDelegate())
			                     .ToDelegate());

		public static IProjection Default<T>(this IFormattedProjection<T> @this, T parameter)
			=> @this.Get(null)(parameter);

		public static ISelect<T, IProjection> Project<T>(this IFormatter<T> @this,
		                                                 params Expression<Func<T, object>>[] expressions)
			=> new Projection<T>(@this, expressions);

		public static Pair<string, Func<T, IProjection>> Entry<T>(this IFormatEntry<T> @this,
		                                                          params Expression<Func<T, object>>[] expressions)
			=> @this.Get().Entry(expressions);

		public static Pair<string, Func<T, IProjection>> Entry<T>(in this Pair<string, Func<T, string>> @this,
		                                                          params Expression<Func<T, object>>[] expressions)
			=> Pairs.Create(@this.Key, new Projection<T>(@this.Value, expressions).ToDelegate());
	}
}