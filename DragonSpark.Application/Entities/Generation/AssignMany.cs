using Bogus;
using DragonSpark.Model.Commands;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class AssignMany<T, TOther> : ICommand<(Faker, T, List<TOther>)>
	{
		readonly Action<Faker, T, List<TOther>> _previous;
		readonly Action<TOther, T>              _assign;

		public AssignMany(Action<Faker, T, List<TOther>> previous, Action<TOther, T> assign)
		{
			_previous = previous;
			_assign   = assign;
		}

		public void Execute((Faker, T, List<TOther>) parameter)
		{
			var (values, owner, instance) = parameter;
			_previous(values, owner, instance);
			foreach (var other in instance)
			{
				_assign(other, owner);
			}
		}
	}
}