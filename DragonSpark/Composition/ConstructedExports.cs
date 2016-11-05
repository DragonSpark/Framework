using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition
{
	public sealed class ConstructedExports : ExportSourceBase<ConstructorInfo>
	{
		public ConstructedExports( IDictionary<Type, ConstructorInfo> constructors ) : base( constructors.Keys, new DictionarySource<Type, ConstructorInfo>( constructors ) ) {}

		public ConstructorInfo Get( IEnumerable<ConstructorInfo> parameter ) => Get( parameter.Select( info => info.DeclaringType ).Distinct().Single() );
	}
}