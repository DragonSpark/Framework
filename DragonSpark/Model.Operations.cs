using System.Threading.Tasks;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ValueTask<T> ToOperation<T>(this Task<T> @this) => new ValueTask<T>(@this);

		public static ValueTask<T> ToOperation<T>(this T @this) => new ValueTask<T>(@this);
	}
}
