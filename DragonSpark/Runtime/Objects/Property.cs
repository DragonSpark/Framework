using System;
using System.Linq.Expressions;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Invocation.Expressions;

namespace DragonSpark.Runtime.Objects
{
	sealed class Property<T> : Select<T, Pair<string, object>>, IProperty<T>
	{
		public Property(Expression<Func<T, object>> expression)
			: base(expression.Compile().Start().Select(new Pairing(expression.GetMemberInfo().Name))) {}
	}
}