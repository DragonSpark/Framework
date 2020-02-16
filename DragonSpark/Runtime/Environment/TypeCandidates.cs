using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace DragonSpark.Runtime.Environment
{
	sealed class TypeCandidates : ISelect<Type, IEnumerable<Type>>, IActivateUsing<Array<Type>>
	{
		readonly Func<Type, bool>                _condition;
		readonly Func<Type, ISelect<Type, Type>> _selections;
		readonly Array<Type>                     _types;

		[UsedImplicitly]
		public TypeCandidates(Array<Type> types) : this(types, Is.Assigned(), Selections.Default.Get) {}

		public TypeCandidates(Array<Type> types, Func<Type, bool> condition, Func<Type, ISelect<Type, Type>> selections)
		{
			_types      = types;
			_condition  = condition;
			_selections = selections;
		}

		public IEnumerable<Type> Get(Type parameter)
		{
			var select = _selections(parameter);
			var length = _types.Length;
			for (var i = 0; i < length; i++)
			{
				var type = select.Get(_types[i]);
				if (_condition(type))
				{
					yield return type;
				}
			}
		}
	}
}