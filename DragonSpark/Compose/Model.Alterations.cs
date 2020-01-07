using DragonSpark.Model.Selection.Alterations;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T Alter<T>(this IEnumerable<IAlteration<T>> @this, T seed)
			=> @this.Aggregate(seed, (current, alteration) => alteration.Get(current));
	}
}