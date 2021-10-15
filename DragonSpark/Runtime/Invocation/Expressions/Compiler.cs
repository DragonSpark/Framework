using DragonSpark.Model.Selection;
using System.Linq.Expressions;

namespace DragonSpark.Runtime.Invocation.Expressions;

sealed class Compiler<T> : Select<Expression<T>, T>
{
	public static Compiler<T> Default { get; } = new Compiler<T>();

	Compiler() : base(x => x.Compile()) {}
}