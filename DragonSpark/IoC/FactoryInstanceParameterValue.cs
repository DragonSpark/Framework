using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using DragonSpark.Objects;
using Microsoft.Practices.ObjectBuilder2;
using System;

namespace DragonSpark.IoC
{
	public class FactoryInstanceParameterValue : Microsoft.Practices.Unity.TypedInjectionValue, IDependencyResolverPolicy
	{
		readonly IFactory factory;
		readonly object source;

		public FactoryInstanceParameterValue( IFactory factory, object source, Type parameterType ) : base( parameterType )
		{
			this.factory = factory;
			this.source = source;
		}

		public override IDependencyResolverPolicy GetResolverPolicy( Type typeToBuild )
		{
			return this;
		}

		public object Resolve( IBuilderContext context )
		{
			var result = factory.Create( context.BuildKey.Type, source );
			return result;
		}
	}

	public class FactoryParameterValue : Microsoft.Practices.Unity.TypedInjectionValue, IDependencyResolverPolicy
	{
		readonly Configuration.NamedTypeBuildKey key;
		readonly object source;

		public FactoryParameterValue( Configuration.NamedTypeBuildKey key, object source, Type parameterType ) : base( parameterType )
		{
			this.key = key;
			this.source = source;
		}

		public override IDependencyResolverPolicy GetResolverPolicy( Type typeToBuild )
		{
			return this;
		}

		public object Resolve( IBuilderContext context )
		{
			var factory = key.Create<IFactory>( context );
			var parameter = source.AsTo<Configuration.NamedTypeBuildKey, object>( x => x.Create<object>( context ) ) ?? source;
			var result = factory.Create( context.BuildKey.Type, parameter );
			return result;
		}
	}
}