using System;
using System.Linq;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Aspects
{
	sealed class Compare<TIn, TOut> : Condition<Array<Type>>
	{
		public static Compare<TIn, TOut> Default { get; } = new Compare<TIn, TOut>();

		Compare() : base(new Compare(A.Type<TIn>(), A.Type<TOut>())) {}
	}

	sealed class Compare : ICondition<Array<Type>>
	{
		readonly Array<ICondition<Type>> _conditions;
		readonly uint                    _length;

		public Compare(params Type[] types) : this(new Array<Type>(types)) {}

		public Compare(Array<Type> types)
			: this(types.Open().Select(x => new IsAssignableFrom(x)).ToArray(), types.Length) {}

		public Compare(Array<ICondition<Type>> conditions, uint length)
		{
			_conditions = conditions;
			_length     = length;
		}

		public bool Get(Array<Type> parameter)
		{
			if (parameter.Length == _length)
			{
				for (var i = 0u; i < _length; i++)
				{
					if (!_conditions[i].Get(parameter[i]))
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}
	}
}