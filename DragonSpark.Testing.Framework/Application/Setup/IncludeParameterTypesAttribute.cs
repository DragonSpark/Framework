using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	[AttributeUsage( AttributeTargets.Method )]
	public class IncludeParameterTypesAttribute : TypeProviderAttributeBase
	{
		public IncludeParameterTypesAttribute( params Type[] additionalTypes ) : base( new Factory( additionalTypes ).Get ) {}

		new sealed class Factory : TypeProviderAttributeBase.Factory
		{
			public Factory( params Type[] additionalTypes ) : base( additionalTypes.ToImmutableArray() ) {}

			public override ImmutableArray<Type> Get( MethodBase parameter ) => base.Get( parameter ).Union( parameter.GetParameterTypes().AsEnumerable() ).ToImmutableArray();
		}
	}
}