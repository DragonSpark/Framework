using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class SingletonDefaultValueProvider : DefaultValueProviderBase
	{
		readonly Type hostType;
		readonly IParameterizedSource<Type, object> provider;
		readonly Func<Type, Func<object, object>> accountedSource;

		public SingletonDefaultValueProvider( Type hostType = null, string propertyName = nameof(SingletonLocator.Default) ) 
			: this( new SingletonLocator( new SingletonDelegates( new SingletonProperties( new SpecifiedSingletonHostSpecification( hostType, propertyName.ToItem() ).Project<SingletonRequest, PropertyInfo>( request => request.Candidate ) ) ).Get ), Sources.Defaults.AccountedSource, hostType ) {}

		[UsedImplicitly]
		public SingletonDefaultValueProvider( IParameterizedSource<Type, object> provider, Func<Type, Func<object, object>> accountedSource, Type hostType = null )
		{
			this.hostType = hostType;
			this.provider = provider;
			this.accountedSource = accountedSource;
		}

		public override object Get( PropertyInfo parameter )
		{
			var singleton = provider.Get( hostType ?? parameter.PropertyType );
			var result = accountedSource( parameter.PropertyType ).Invoke( singleton );
			return result;
		}
	}
}