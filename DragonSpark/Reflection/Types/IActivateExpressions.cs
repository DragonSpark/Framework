using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Reflection.Types
{
	public interface IActivateExpressions : ISelect<Type, Expression>
	{
		IArray<ParameterExpression> Parameters { get; }
	}
}