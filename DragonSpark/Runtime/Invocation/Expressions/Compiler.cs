using System.Linq.Expressions;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	sealed class Compiler<T> : Select<Expression<T>, T>
	{
		public static Compiler<T> Default { get; } = new Compiler<T>();

		Compiler() : base(x => x.Compile()) {}
	}
}