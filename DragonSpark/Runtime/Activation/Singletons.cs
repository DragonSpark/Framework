using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Runtime.Activation
{
	public sealed class Singletons : ReferenceValueStore<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(Start.A.Selection.Of.System.Type.By.Default<object>()
		                         .Unless(A.This(HasSingletonProperty.Default),
		                                 A.This(SingletonProperty.Default)
		                                  .Select(SingletonPropertyDelegates.Default)
		                                  .Then()
		                                  .Invoke()
		                                  .Get())
		                         .Get) {}
	}
}