using System;
using System.Linq.Expressions;

namespace DragonSpark.Model.Selection
{
	public class Compile<TIn, TOut> : Select<TIn, TOut>
	{
		public Compile(Expression<Func<TIn, TOut>> select) : base(select.Compile()) {}
	}
}