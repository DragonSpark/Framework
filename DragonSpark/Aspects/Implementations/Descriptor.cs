using System;
using System.Linq;
using DragonSpark.Aspects.Build;
using DragonSpark.Extensions;
using DragonSpark.Specifications;
using PostSharp.Aspects;

namespace DragonSpark.Aspects.Implementations
{
	public class Descriptor<T> : TypeBasedAspectInstanceLocator<T>, IDescriptor where T : IAspect
	{
		public Descriptor( Type declaringType, params Type[] implementedTypes ) : base( TypeAssignableSpecification.Defaults.Get( declaringType ).And( new AllSpecification<Type>( implementedTypes.Select( type => TypeAssignableSpecification.Defaults.Get( type ).Inverse() ).Fixed() ) ) )
		{
			DeclaringType = declaringType;
		}

		public Type DeclaringType { get; }
	}
}