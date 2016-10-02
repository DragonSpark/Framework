using System;
using System.Collections.Immutable;
using System.Reflection;
using DragonSpark.Extensions;

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