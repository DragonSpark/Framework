using Bogus;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class Assign<T, TOther> : ICommand<(Faker, T, TOther)>
	{
		readonly Action<Faker, T, TOther> _previous;
		readonly Action<TOther, T>        _assign;

		public Assign(Action<Faker, T, TOther> previous, Action<TOther, T> assign)
		{
			_previous = previous;
			_assign   = assign;
		}

		public void Execute((Faker, T, TOther) parameter)
		{
			var (values, owner, instance) = parameter;
			_previous(values, owner, instance);
			_assign(instance, owner);
		}
	}
}