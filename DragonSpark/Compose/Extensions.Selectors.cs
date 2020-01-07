﻿using DragonSpark.Compose.Model;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public partial class Extensions
	{
		public static ResultContext<T> Bind<_, T>(this Selector<_, T> @this, _ parameter)
			=> new FixedSelection<_, T>(@this, parameter).Then();

		public static ResultContext<T> Bind<_, T>(this Selector<_, T> @this, Func<_> parameter)
			=> new DelegatedSelection<_, T>(@this, parameter).Then();

		public static ResultContext<T> Bind<_, T>(this Selector<_, T> @this, IResult<_> parameter)
			=> @this.Bind(parameter.Get);

		public static Selector<T, string> Bind<T>(this Selector<Type, string> @this)
			=> @this.Bind(A.Type<T>()).Accept<T>();
	}
}