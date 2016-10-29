using DragonSpark.Activation;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using Type = System.Type;

namespace DragonSpark.TypeSystem
{
	[Export, Shared]
	public class AllTypesOfFactory : ParameterizedSourceBase<Type, Array>
	{
		readonly ImmutableArray<Type> types;
		readonly IActivator activator;

		public AllTypesOfFactory( ImmutableArray<Type> types, IActivator activator )
		{
			this.types = types;
			this.activator = activator;
		}

		public ImmutableArray<T> Create<T>() => Get( typeof(T) ).Cast<T>().ToImmutableArray();

		public override Array Get( Type parameter ) => activator.ActivateMany<object>( parameter, types.ToArray() ).ToArray();
	}
}