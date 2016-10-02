using DragonSpark.Extensions;
using System.Runtime.InteropServices;
using DragonSpark.Coercion;

namespace DragonSpark.Sources
{
	public sealed class SourceCoercer<T> : ICoercer<T>
	{
		public static SourceCoercer<T> Default { get; } = new SourceCoercer<T>();
		SourceCoercer() {}

		public T Coerce( [Optional]object parameter )
		{
			var source = parameter as ISource;
			var value = source?.Get();

			var result = value is T ? (T)value : parameter.As<T>();
			return result;
		}
	}
}