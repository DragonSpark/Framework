using System.Linq.Expressions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	sealed class Parameters<T> : ReferenceValueStore<string, ParameterExpression>
	{
		public static Parameters<T> Default { get; } = new Parameters<T>();

		Parameters() : base(new Parameter(Type<T>.Instance).Get) {}
	}
}