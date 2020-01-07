﻿using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Runtime.Activation
{
	public sealed class Singletons : ReferenceValueStore<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(Start.A.Selection.Of.System.Type.By.Default<object>()
		                         .Then()
		                         .Or.Use(Start.An.Instance(SingletonProperty.Default)
		                                      .Select(SingletonPropertyDelegates.Default)
		                                      .Then()
		                                      .Invoke()
		                                      .Get())
		                         .When(HasSingletonProperty.Default)
		                         .Get) {}
	}
}