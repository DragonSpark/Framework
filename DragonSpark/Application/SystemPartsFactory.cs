using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Application
{
	public sealed class SystemPartsFactory : ParameterizedSourceBase<IEnumerable<Type>, SystemParts>
	{
		public static SystemPartsFactory Default { get; } = new SystemPartsFactory();
		SystemPartsFactory() {}

		public override SystemParts Get( IEnumerable<Type> parameter ) => new SystemParts( parameter.Distinct().Prioritize( type => AssemblyPriorityLocator.Default.Get( type.Assembly() ) ) );
	}
}