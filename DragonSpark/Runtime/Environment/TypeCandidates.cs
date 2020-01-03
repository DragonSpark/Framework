﻿using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace DragonSpark.Runtime.Environment
{
	sealed class TypeCandidates : ISelect<Type, IEnumerable<Type>>, IActivateUsing<Array<Type>>
	{
		readonly ICondition<Type>                _condition;
		readonly Func<Type, ISelect<Type, Type>> _selections;
		readonly Array<Type>                     _types;

		[UsedImplicitly]
		public TypeCandidates(Array<Type> types) : this(types, Is.Assigned(), Selections.Default.Get) {}

		public TypeCandidates(Array<Type> types, ICondition<Type> condition,
		                              Func<Type, ISelect<Type, Type>> selections)
		{
			_types      = types;
			_condition  = condition;
			_selections = selections;
		}

		public IEnumerable<Type> Get(Type parameter)
		{
			var select = _selections(parameter);
			var length = _types.Length;
			for (var i = 0u; i < length; i++)
			{
				var type = select.Get(_types[i]);
				if (_condition.Get(type))
				{
					yield return type;
				}
			}
		}
	}
}