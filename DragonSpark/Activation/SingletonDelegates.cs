using System;
using System.Reflection;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Activation
{
	public class SingletonDelegates<T> : FactoryCache<Type, T>
	{
		readonly Func<Type, PropertyInfo> propertySource;
		readonly Func<PropertyInfo, T> source;

		public SingletonDelegates( Func<Type, PropertyInfo> propertySource, Func<PropertyInfo, T> source )
		{
			this.propertySource = propertySource;
			this.source = source;
		}

		protected override T Create( Type parameter )
		{
			var property = propertySource( parameter );
			var result = property != null ? source( property ) : default(T);
			return result;
		}
	}
}