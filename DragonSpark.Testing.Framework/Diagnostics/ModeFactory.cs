using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Testing.Framework.Diagnostics
{
	sealed class ModeFactory<T> : ParameterizedSourceBase<ImmutableArray<T>, T>
	{
		public static ModeFactory<T> Default { get; } = new ModeFactory<T>();
		public override T Get( ImmutableArray<T> parameter ) => parameter.ToArray().GroupBy( n => n ).OrderByDescending( g => g.Count() ).Select( g => g.Key ).FirstOrDefault();
	}
}