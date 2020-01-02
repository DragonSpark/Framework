using System;
using AutoFixture.Kernel;
using DragonSpark.Compose;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Application.Hosting.xUnit
{
	public class Specimen<T> : ISpecimenBuilder
	{
		readonly static Func<object, bool> Condition = Start.An.Instance(IsOf<Type>.Default)
		                                                    .Then()
		                                                    .And(Start.A.Selection.Of.Any.AndOf<Type>()
		                                                              .By.Cast.Or.Throw.Then()
		                                                              .Select(Type<T>.Instance.Equal())
		                                                              .Selector());

		readonly Func<object, bool> _condition;
		readonly NoSpecimen         _none;
		readonly Func<T>            _specimen;

		public Specimen(Func<T> specimen) : this(Condition, specimen, NoSpecimenInstance.Default) {}

		public Specimen(Func<object, bool> condition, Func<T> specimen, NoSpecimen none)
		{
			_condition = condition;
			_specimen  = specimen;
			_none      = none;
		}

		public object Create(object request, ISpecimenContext context)
			=> _condition(request) ? (object)_specimen() : _none;
	}
}