using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonSpark.Runtime
{
	public sealed class EnumerableResultAlteration<T> : AlterationBase<IEnumerable<T>>
	{
		public static EnumerableResultAlteration<T> Default { get; } = new EnumerableResultAlteration<T>();
		EnumerableResultAlteration() {}

		public override IEnumerable<T> Get( [Optional]IEnumerable<T> parameter ) => parameter ?? Items<T>.Default;
	}
}
