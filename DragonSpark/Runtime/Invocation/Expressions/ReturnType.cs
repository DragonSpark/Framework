using System;
using DragonSpark.Model.Results;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	public sealed class ReturnType<T> : Instance<Type>
	{
		public static ReturnType<T> Default { get; } = new ReturnType<T>();

		ReturnType() : base(Type<T>.Metadata.GetDeclaredMethod(nameof(Func<object>.Invoke)).ReturnType) {}
	}
}