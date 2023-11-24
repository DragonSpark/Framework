using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System.Linq.Expressions;

namespace DragonSpark.Runtime.Invocation.Expressions;

sealed class Parameters<T> : ReferenceValueStore<string, ParameterExpression>
{
	public static Parameters<T> Default { get; } = new();

	Parameters() : base(new Parameter(A.Type<T>()).Get) {}
}