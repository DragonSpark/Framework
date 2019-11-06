using System;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Compose.Generics
{
	public sealed class Extent
	{
		readonly Type _definition;

		public Extent(Type definition) => _definition = definition;

		public Extent<T> Type<T>() => new Extent<T>(_definition);
	}

	public class Extent<T> : IGeneric<T>
	{
		readonly Type _definition;

		public Extent(Type definition) => _definition = definition;

		public Selections As => new Selections(_definition);

		public Func<T> Get(Array<Type> parameter) => new Generic<T>(_definition).Get(parameter);

		public Extent<T1, T> WithParameterOf<T1>() => new Extent<T1, T>(_definition);

		public class Selections
		{
			readonly Type _definition;

			public Selections(Type definition) => _definition = definition;

			public SequenceExtent<T> Sequence() => new SequenceExtent<T>(_definition);

			public Extent<Func<T>> Delegate() => new Extent<Func<T>>(_definition);

			public Extent<ICondition<T>> Condition() => new Extent<ICondition<T>>(_definition);

			public Extent<IResult<T>> Result() => new Extent<IResult<T>>(_definition);

			public Extent<ICommand<T>> Command() => new Extent<ICommand<T>>(_definition);
		}
	}

	public class Extent<T1, T> : IGeneric<T1, T>
	{
		readonly Type _definition;

		public Extent(Type definition) => _definition = definition;

		public Func<T1, T> Get(Array<Type> parameter) => new Generic<T1, T>(_definition).Get(parameter);

		public Selections As() => new Selections(_definition);

		public Extent<T1, T2, T> AndOf<T2>() => new Extent<T1, T2, T>(_definition);

		public class Selections
		{
			readonly Type _definition;

			public Selections(Type definition) => _definition = definition;

			public SequenceExtent<T1, T> Sequence() => new SequenceExtent<T1, T>(_definition);

			public Extent<T1, Func<T>> Delegate() => new Extent<T1, Func<T>>(_definition);

			public Extent<T1, ICondition<T>> Condition() => new Extent<T1, ICondition<T>>(_definition);

			public Extent<T1, IResult<T>> Result() => new Extent<T1, IResult<T>>(_definition);

			public Extent<T1, ICommand<T>> Command() => new Extent<T1, ICommand<T>>(_definition);
		}
	}

	public class Extent<T1, T2, T> : IGeneric<T1, T2, T>
	{
		readonly Type _definition;

		public Extent(Type definition) => _definition = definition;

		public Func<T1, T2, T> Get(Array<Type> parameter) => new Generic<T1, T2, T>(_definition).Get(parameter);

		public Selections As() => new Selections(_definition);

		public Extent<T1, T2, T3, T> AndOf<T3>() => new Extent<T1, T2, T3, T>(_definition);

		public class Selections
		{
			readonly Type _definition;

			public Selections(Type definition) => _definition = definition;

			public SequenceExtent<T1, T2, T> Sequence() => new SequenceExtent<T1, T2, T>(_definition);

			public Extent<T1, T2, Func<T>> Delegate() => new Extent<T1, T2, Func<T>>(_definition);

			public Extent<T1, T2, ICondition<T>> Condition() => new Extent<T1, T2, ICondition<T>>(_definition);

			public Extent<T1, T2, IResult<T>> Result() => new Extent<T1, T2, IResult<T>>(_definition);

			public Extent<T1, T2, ICommand<T>> Command() => new Extent<T1, T2, ICommand<T>>(_definition);
		}
	}

	public class Extent<T1, T2, T3, T> : IGeneric<T1, T2, T3, T>
	{
		readonly Type _definition;

		public Extent(Type definition) => _definition = definition;

		public Selections As => new Selections(_definition);

		public Func<T1, T2, T3, T> Get(Array<Type> parameter) => new Generic<T1, T2, T3, T>(_definition).Get(parameter);

		public Extent<T1, T2, T3, T4, T> AndOf<T4>() => new Extent<T1, T2, T3, T4, T>(_definition);

		public class Selections
		{
			readonly Type _definition;

			public Selections(Type definition) => _definition = definition;

			public SequenceExtent<T1, T2, T3, T> Sequence() => new SequenceExtent<T1, T2, T3, T>(_definition);

			public Extent<T1, T2, T3, Func<T>> Delegate() => new Extent<T1, T2, T3, Func<T>>(_definition);

			public Extent<T1, T2, T3, ICondition<T>> Condition() => new Extent<T1, T2, T3, ICondition<T>>(_definition);

			public Extent<T1, T2, T3, IResult<T>> Result() => new Extent<T1, T2, T3, IResult<T>>(_definition);

			public Extent<T1, T2, T3, ICommand<T>> Command() => new Extent<T1, T2, T3, ICommand<T>>(_definition);
		}
	}

	public class Extent<T1, T2, T3, T4, T> : IGeneric<T1, T2, T3, T4, T>
	{
		readonly Type _definition;

		public Extent(Type definition) => _definition = definition;

		public Selections As => new Selections(_definition);

		public Func<T1, T2, T3, T4, T> Get(Array<Type> parameter)
			=> new Generic<T1, T2, T3, T4, T>(_definition).Get(parameter);

		public class Selections
		{
			readonly Type _definition;

			public Selections(Type definition) => _definition = definition;

			public SequenceExtent<T1, T2, T3, T4, T> Sequence() => new SequenceExtent<T1, T2, T3, T4, T>(_definition);

			public Extent<T1, T2, T3, T4, Func<T>> Delegate() => new Extent<T1, T2, T3, T4, Func<T>>(_definition);

			public Extent<T1, T2, T3, T4, ICondition<T>> Condition()
				=> new Extent<T1, T2, T3, T4, ICondition<T>>(_definition);

			public Extent<T1, T2, T3, T4, IResult<T>> Result() => new Extent<T1, T2, T3, T4, IResult<T>>(_definition);

			public Extent<T1, T2, T3, T4, ICommand<T>> Command()
				=> new Extent<T1, T2, T3, T4, ICommand<T>>(_definition);
		}
	}
}