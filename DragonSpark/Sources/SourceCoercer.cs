using DragonSpark.Coercion;
using System.Runtime.InteropServices;

namespace DragonSpark.Sources
{
	public sealed class SourceCoercer : ICoercer<object>
	{
		public static SourceCoercer Default { get; } = new SourceCoercer();
		SourceCoercer() {}

		public object Coerce( [Optional]object parameter )
		{
			var source = parameter as ISource;
			var result = source?.Get() ?? source;
			return result;
		}
	}
}