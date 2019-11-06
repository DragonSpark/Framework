using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T Alter<T>(this IEnumerable<IAlteration<T>> @this, T seed)
			=> @this.Aggregate(seed, (current, alteration) => alteration.Get(current));

		public static IAlteration<T> ToAlteration<T>(this ISelect<T, T> @this) => @this.ToDelegate().ToAlteration();

		public static IAlteration<T> ToAlteration<T>(this Func<T, T> @this) => Alterations<T>.Default.Get(@this);
	}
}