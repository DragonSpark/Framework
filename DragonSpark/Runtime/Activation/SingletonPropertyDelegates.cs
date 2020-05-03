using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Activation
{
	sealed class SingletonPropertyDelegates : Select<PropertyInfo, Func<object>>
	{
		public static SingletonPropertyDelegates Default { get; } = new SingletonPropertyDelegates();

		SingletonPropertyDelegates() : base(Start.A.Selection<PropertyInfo>()
		                                         .By.Calling(x => x.GetMethod)
		                                         .Select(MethodDelegates<Func<object>>.Default!)
		                                         .Get()
		                                         .Then()
		                                         .Singleton()) {}
	}
}