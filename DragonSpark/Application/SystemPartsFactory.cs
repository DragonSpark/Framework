using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Application
{
	public sealed class SystemPartsFactory : ParameterizedSourceBase<ImmutableArray<Type>, SystemParts>
	{
		public static SystemPartsFactory Default { get; } = new SystemPartsFactory();
		SystemPartsFactory() : this( AssemblyPriorityLocator.Default.Get ) {}
		
		readonly Func<Type, IPriorityAware> locator;

		SystemPartsFactory( Func<Type, IPriorityAware> locator )
		{
			this.locator = locator;
		}

		public override SystemParts Get( ImmutableArray<Type> parameter ) => new SystemParts( parameter.Distinct().Prioritize( locator ) );
	}
}