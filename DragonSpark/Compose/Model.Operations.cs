using DragonSpark.Model.Operations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T> ToOperation<T>(this Task<T> @this) => new ValueTask<T>(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T> ToOperation<T>(this T @this) => new ValueTask<T>(@this);

		public static Task Promote(this IOperation @this) => @this.Get().AsTask();

		public static Task Promote<T>(this IOperation<T> @this, T parameter) => @this.Get(parameter).AsTask();
	}
}