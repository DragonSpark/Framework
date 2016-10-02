using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DragonSpark.Aspects.Build;
using DragonSpark.Extensions;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using PostSharp.Aspects;

namespace DragonSpark.Aspects.Relay
{
	sealed class Support : DelegatedSpecification<Type>, ISupportDefinition
	{
		public static Support Default { get; } = new Support();
		Support() : this( Descriptors.Default.ToImmutableArray() ) {}

		readonly ImmutableArray<IDescriptor> descriptors;

		Support( ImmutableArray<IDescriptor> descriptors ) : base( SpecificationFactory.Default.Get( descriptors.ToArray() ) )
		{
			this.descriptors = descriptors;
		}

		public IEnumerable<AspectInstance> Get( Type parameter )
		{
			foreach ( var descriptor in descriptors )
			{
				var instances = descriptor.Get( parameter ).Fixed();
				if ( instances.Any() )
				{
					return instances;
				}
			}
			return Items<AspectInstance>.Default;
		}
	}
}