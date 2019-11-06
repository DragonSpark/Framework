using System;
using System.Reflection;
using DragonSpark.Runtime.Invocation.Expressions;

namespace DragonSpark.Runtime.Activation
{
	sealed class Constructors<T> : Delegates<ConstructorInfo, Func<T>>
	{
		public static Constructors<T> Default { get; } = new Constructors<T>();

		Constructors() : base(ConstructorExpressions.Default) {}
	}
}