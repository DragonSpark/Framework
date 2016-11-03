using DragonSpark.Sources.Parameterized;
using System.Runtime.InteropServices;

namespace DragonSpark.Sources
{
	public sealed class SourceCoercer : IParameterizedSource<object>
	{
		public static SourceCoercer Default { get; } = new SourceCoercer();
		SourceCoercer() {}

		public object Get( [Optional]object parameter )
		{
			var source = parameter as ISource;
			var result = source?.Get() ?? parameter;
			return result;
		}
	}
}