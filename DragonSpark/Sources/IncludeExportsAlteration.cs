using DragonSpark.Sources.Parameterized;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Sources
{
	public sealed class IncludeExportsAlteration<T> : AlterationBase<IEnumerable<T>>
	{
		public static IncludeExportsAlteration<T> Default { get; } = new IncludeExportsAlteration<T>();
		IncludeExportsAlteration() {}

		public override IEnumerable<T> Get( IEnumerable<T> parameter ) => parameter.Concat( ExportSource<T>.Default );
	}
}