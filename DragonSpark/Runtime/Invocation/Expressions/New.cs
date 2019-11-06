using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	sealed class New : Invocation0<ConstructorInfo, IEnumerable<Expression>, Expression>
	{
		public New(IEnumerable<Expression> parameter) : base(Expression.New, parameter) {}
	}
}