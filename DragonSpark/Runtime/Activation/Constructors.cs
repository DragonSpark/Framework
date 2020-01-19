using DragonSpark.Runtime.Invocation.Expressions;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Activation
{
	sealed class Constructors<T> : Delegates<ConstructorInfo, Func<T>>
	{
		public static Constructors<T> Default { get; } = new Constructors<T>();

		Constructors() : base(ConstructorExpressions.Default) {}
	}
}