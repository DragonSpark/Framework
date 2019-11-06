using System;
using System.Linq.Expressions;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Reflection.Types
{
	public interface IActivateExpressions : ISelect<Type, Expression>
	{
		IArray<ParameterExpression> Parameters { get; }
	}
}