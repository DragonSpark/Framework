using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Alterations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T Alter<T>(this IEnumerable<IAlteration<T>> @this, T seed)
			=> @this.Aggregate(seed, (current, alteration) => alteration.Get(current));

		public static ValueTask<T> Alter<T>(this IEnumerable<IAltering<T>> @this, T seed)
			=> DragonSpark.Model.Operations.Alter<T>.Default.Get(seed, @this.Result());
	}
}