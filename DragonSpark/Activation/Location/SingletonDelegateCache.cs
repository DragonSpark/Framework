using System;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Activation.Location
{
	sealed class SingletonDelegateCache : FactoryCache<PropertyInfo, Func<object>>
	{
		public static SingletonDelegateCache Default { get; } = new SingletonDelegateCache();
		SingletonDelegateCache() {}

		protected override Func<object> Create( PropertyInfo parameter ) => 
			parameter.PropertyType.Adapt().IsGenericOf( typeof(ISource<>), false ) ? parameter.GetMethod.CreateDelegate<Func<ISource>>().Invoke().Get : parameter.GetMethod.CreateDelegate<Func<object>>();
	}
}