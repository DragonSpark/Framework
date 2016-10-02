using System;
using System.Collections.Immutable;
using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Method )]
	public abstract class TypeProviderAttributeBase : HostingAttributeBase
	{
		protected TypeProviderAttributeBase( params Type[] types ) : this( types.ToImmutableArray() ) {}
		protected TypeProviderAttributeBase( ImmutableArray<Type> additionalTypes ) : this( new Factory( additionalTypes ).Get ) {}

		protected TypeProviderAttributeBase( Func<MethodBase, ImmutableArray<Type>> factory ) : this( factory.Wrap() ) {}
		protected TypeProviderAttributeBase( Func<object, Func<MethodBase, ImmutableArray<Type>>> provider ) : base( provider ) {}

		protected class Factory : ParameterizedSourceBase<MethodBase, ImmutableArray<Type>>
		{
			readonly ImmutableArray<Type> additionalTypes;
			public Factory( ImmutableArray<Type> additionalTypes )
			{
				this.additionalTypes = additionalTypes;
			}

			public override ImmutableArray<Type> Get( MethodBase parameter ) => additionalTypes;
		}
	}
}