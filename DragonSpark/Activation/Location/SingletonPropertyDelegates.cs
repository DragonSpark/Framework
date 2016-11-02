using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	sealed class SingletonPropertyDelegates : ParameterizedSourceBase<PropertyInfo, Func<object>>
	{
		public static SingletonPropertyDelegates Default { get; } = new SingletonPropertyDelegates();
		SingletonPropertyDelegates() {}

		public override Func<object> Get( PropertyInfo parameter ) => 
			/*parameter.PropertyType.Adapt().IsGenericOf( typeof(ISource<>) ) ? parameter.GetMethod.CreateDelegate<Func<ISource>>().Invoke().Get :*/ parameter.GetMethod.CreateDelegate<Func<object>>().Cache();
	}
}