using DragonSpark.Aspects.Build;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Aspects.Relay
{
	public sealed class Descriptors : ItemSource<IDescriptor>
	{
		public static Descriptors Default { get; } = new Descriptors();
		Descriptors() : this( CommandDescriptor.Default, SourceDescriptor.Default, SpecificationDescriptor.Default ) {}

		Descriptors( params IDescriptor[] descriptors ) : this( descriptors.Select( definition => definition.DeclaringType.Adapt() ).ToArray(), descriptors ) {}
		Descriptors( TypeAdapter[] adapters, IDescriptor[] descriptors ) 
			: this( descriptors, new TypedPairs<IAspect>( adapters.Tuple( descriptors.Select( descriptor => new Func<object, IAspect>( descriptor.Get ) ).ToArray() ) ) ) {}

		Descriptors( IEnumerable<IDescriptor> descriptors, ITypedPairs<IAspect> instances ) : base( descriptors )
		{
			Aspects = instances;
		}

		public ITypedPairs<IAspect> Aspects { get; }
	}
}