using AutoFixture.Kernel;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Hosting.xUnit
{
	public sealed class SingletonQuery : ISelect<Type, IEnumerable<IMethod>>, IMethodQuery
	{
		public static SingletonQuery Default { get; } = new SingletonQuery();

		SingletonQuery() : this(HasSingletonProperty.Default) {}

		readonly ICondition<Type> _condition;

		public SingletonQuery(ICondition<Type> condition) => _condition = condition;

		IEnumerable<IMethod> IMethodQuery.SelectMethods(Type type) => Get(type);

		public IEnumerable<IMethod> Get(Type parameter)
		{
			if (_condition.Get(parameter))
			{
				yield return new SingletonMethod(parameter);
			}
		}
	}
}