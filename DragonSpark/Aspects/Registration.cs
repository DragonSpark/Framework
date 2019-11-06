using System;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Aspects
{
	public sealed class Registration : IRegistration
	{
		readonly Func<Array<Type>, Func<object>> _source;

		public Registration(Type definition) : this(Always<Array<Type>>.Default, definition) {}

		public Registration(ICondition<Array<Type>> condition, Type definition)
			: this(condition, AspectOpenGeneric.Default.Get(definition)) {}

		public Registration(ICondition<Array<Type>> condition, Func<Array<Type>, Func<object>> source)
		{
			_source   = source;
			Condition = condition;
		}

		public ICondition<Array<Type>> Condition { get; }

		public object Get(Array<Type> parameter) => _source(parameter)();
	}

	public class Registration<TIn, TOut> : FixedResult<Array<Type>, object>, IRegistration
	{
		public Registration(IAspect<TIn, TOut> aspect) : this(Compare<TIn, TOut>.Default, aspect) {}

		public Registration(ICondition<Array<Type>> condition, IAspect<TIn, TOut> aspect) : base(aspect)
			=> Condition = condition;

		public ICondition<Array<Type>> Condition { get; }
	}
}