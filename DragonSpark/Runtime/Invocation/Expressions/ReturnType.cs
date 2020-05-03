using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	public sealed class ReturnType<T> : Instance<Type>
	{
		public static ReturnType<T> Default { get; } = new ReturnType<T>();

		ReturnType() : base(A.Metadata<T>().GetDeclaredMethod(nameof(Func<object>.Invoke))!.ReturnType) {}
	}
}