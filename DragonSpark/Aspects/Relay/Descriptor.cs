using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Aspects.Build;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using PostSharp.Aspects;

namespace DragonSpark.Aspects.Relay
{
	public class Descriptor<T> : TypeBasedAspectInstanceLocator<T>, IDescriptor where T : IAspect
	{
		readonly Func<object, object> adapterSource;
		readonly Func<object, IAspect> aspectSource;
		readonly ImmutableArray<IAspectInstanceLocator> locators;
		
		public Descriptor( ITypeAware source, ITypeAware destination, Type adapterType, Type introducedInterface, params IAspectInstanceLocator[] locators ) 
			: this( source,
					new AdapterFactorySource( destination.DeclaringType, adapterType ).Get, 
					ParameterConstructor<object, IAspect>.Make( introducedInterface, typeof(T) ), 
					locators.ToImmutableArray()
			) {}

		Descriptor( ITypeAware source, Func<object, object> adapterSource, Func<object, IAspect> aspectSource, ImmutableArray<IAspectInstanceLocator> locators ) : base( source )
		{
			DeclaringType = source.DeclaringType;
			this.adapterSource = adapterSource;
			this.aspectSource = aspectSource;
			this.locators = locators;
		}

		public Type DeclaringType { get; }

		public IAspect Get( object parameter ) => aspectSource( adapterSource( parameter ) );

		IEnumerable<AspectInstance> IParameterizedSource<Type, IEnumerable<AspectInstance>>.Get( Type parameter )
		{
			var methods = GetMappings( parameter ).ToArray();
			var result = methods.Length == locators.Length ? base.Get( parameter ).Append( methods ).Fixed() : Items<AspectInstance>.Default;
			return result;
		}

		IEnumerable<AspectInstance> GetMappings( Type parameter )
		{
			foreach ( var locator in locators )
			{
				var instance = locator.Get( parameter );
				if ( instance != null )
				{
					yield return instance;
				}
			}
		}
	}
}