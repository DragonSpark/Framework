using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	public class SingletonProperties : ParameterizedSourceBase<Type, PropertyInfo>
	{
		public static IParameterizedSource<Type, PropertyInfo> Default { get; } = new SingletonProperties().ToCache();
		SingletonProperties() : this( Defaults.SourcedSingleton ) {}

		readonly ISpecification<SingletonRequest> specification;

		public SingletonProperties( ISpecification<SingletonRequest> specification )
		{
			this.specification = specification;
		}

		public override PropertyInfo Get( Type parameter )
		{
			foreach ( var property in parameter.GetTypeInfo().DeclaredProperties.Fixed() )
			{
				var isSatisfiedBy = specification.IsSatisfiedBy( new SingletonRequest( parameter, property ) );
				if ( isSatisfiedBy )
				{
					return property;
				}
			}
			return null;
		}
	}
}