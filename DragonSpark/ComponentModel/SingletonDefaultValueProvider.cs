using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class SingletonDefaultValueProvider : DefaultValueProviderBase
	{
		readonly Type hostType;
		readonly IParameterizedSource<Type, object> provider;
		
		public SingletonDefaultValueProvider( Type hostType = null, string propertyName = nameof(SingletonLocator.Default) ) : this( new SingletonLocator( new SingletonDelegates( new SingletonProperties( new SpecifiedSingletonHostSpecification( hostType, propertyName.ToItem() ).Project<SingletonRequest, PropertyInfo>( request => request.Candidate ) ) ).Get ), hostType ) {}

		SingletonDefaultValueProvider( IParameterizedSource<Type, object> provider, Type hostType = null )
		{
			this.hostType = hostType;
			this.provider = provider;
		}

		public override object Get( DefaultValueParameter parameter ) => provider.Get( hostType ?? parameter.Metadata.PropertyType );
	}
}