using DragonSpark.Aspects.Build;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;
using System;
using System.Collections.Generic;

namespace DragonSpark.Aspects
{
	[AttributeUsage( AttributeTargets.Class ), AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	public abstract class ApplyInstanceAspectBase : InstanceLevelAspect, IAspectProvider
	{
		readonly Func<Type, bool> specification;
		readonly Func<Type, IEnumerable<AspectInstance>> source;

		protected ApplyInstanceAspectBase( ISupportDefinition definition ) : this( definition.IsSatisfiedBy, definition.Get ) {}

		protected ApplyInstanceAspectBase( Func<Type, bool> specification, Func<Type, IEnumerable<AspectInstance>> source )
		{
			this.specification = specification;
			this.source = source;
		}

		public override bool CompileTimeValidate( Type type ) => specification( type );

		IEnumerable<AspectInstance> IAspectProvider.ProvideAspects( object targetElement ) => source( (Type)targetElement );
	}
}