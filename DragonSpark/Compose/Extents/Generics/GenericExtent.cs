using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Extents.Generics;

public sealed class GenericExtent
{
	readonly Type _definition;

	public GenericExtent(Type definition) => _definition = definition;

	public GenericExtent<T> Type<T>() => new(_definition);
}

public class GenericExtent<T> : IGeneric<T>
{
	readonly Type _definition;

	public GenericExtent(Type definition) => _definition = definition;

	public Selections As => new(_definition);

	public GenericExtent<T1, T> WithParameterOf<T1>() => new(_definition);

	public Func<T> Get(Array<Type> parameter) => new Generic<T>(_definition).Get(parameter);

	public class Selections
	{
		readonly Type _definition;

		public Selections(Type definition) => _definition = definition;

		public GenericSequenceExtent<T> Sequence() => new(_definition);

		public GenericExtent<Func<T>> Delegate() => new(_definition);

		public GenericExtent<ICondition<T>> Condition() => new(_definition);

		public GenericExtent<IResult<T>> Result() => new(_definition);

		public GenericExtent<ICommand<T>> Command() => new(_definition);
	}
}

public class GenericExtent<T1, T> : IGeneric<T1, T>
{
	readonly Type _definition;

	public GenericExtent(Type definition) => _definition = definition;

	public Selections As() => new(_definition);

	public GenericExtent<T1, T2, T> AndOf<T2>() => new(_definition);

	public Func<T1, T> Get(Array<Type> parameter) => new Generic<T1, T>(_definition).Get(parameter);

	public class Selections
	{
		readonly Type _definition;

		public Selections(Type definition) => _definition = definition;

		public GenericSequenceExtent<T1, T> Sequence() => new(_definition);

		public GenericExtent<T1, Func<T>> Delegate() => new(_definition);

		public GenericExtent<T1, ICondition<T>> Condition() => new(_definition);

		public GenericExtent<T1, IResult<T>> Result() => new(_definition);

		public GenericExtent<T1, ICommand<T>> Command() => new(_definition);
	}
}

public class GenericExtent<T1, T2, T> : IGeneric<T1, T2, T>
{
	readonly Type _definition;

	public GenericExtent(Type definition) => _definition = definition;

	public Selections As() => new(_definition);

	public GenericExtent<T1, T2, T3, T> AndOf<T3>() => new(_definition);

	public Func<T1, T2, T> Get(Array<Type> parameter) => new Generic<T1, T2, T>(_definition).Get(parameter);

	public class Selections
	{
		readonly Type _definition;

		public Selections(Type definition) => _definition = definition;

		public GenericSequenceExtent<T1, T2, T> Sequence() => new(_definition);

		public GenericExtent<T1, T2, Func<T>> Delegate() => new(_definition);

		public GenericExtent<T1, T2, ICondition<T>> Condition() => new(_definition);

		public GenericExtent<T1, T2, IResult<T>> Result() => new(_definition);

		public GenericExtent<T1, T2, ICommand<T>> Command() => new(_definition);
	}
}

public class GenericExtent<T1, T2, T3, T> : IGeneric<T1, T2, T3, T>
{
	readonly Type _definition;

	public GenericExtent(Type definition) => _definition = definition;

	public Selections As => new(_definition);

	public GenericExtent<T1, T2, T3, T4, T> AndOf<T4>() => new(_definition);

	public Func<T1, T2, T3, T> Get(Array<Type> parameter) => new Generic<T1, T2, T3, T>(_definition).Get(parameter);

	public class Selections
	{
		readonly Type _definition;

		public Selections(Type definition) => _definition = definition;

		public GenericSequenceExtent<T1, T2, T3, T> Sequence() => new(_definition);

		public GenericExtent<T1, T2, T3, Func<T>> Delegate() => new(_definition);

		public GenericExtent<T1, T2, T3, ICondition<T>> Condition() => new(_definition);

		public GenericExtent<T1, T2, T3, IResult<T>> Result() => new(_definition);

		public GenericExtent<T1, T2, T3, ICommand<T>> Command() => new(_definition);
	}
}

public class GenericExtent<T1, T2, T3, T4, T> : IGeneric<T1, T2, T3, T4, T>
{
	readonly Type _definition;

	public GenericExtent(Type definition) => _definition = definition;

	public Selections As => new(_definition);

	public Func<T1, T2, T3, T4, T> Get(Array<Type> parameter)
		=> new Generic<T1, T2, T3, T4, T>(_definition).Get(parameter);

	public class Selections
	{
		readonly Type _definition;

		public Selections(Type definition) => _definition = definition;

		public GenericSequenceExtent<T1, T2, T3, T4, T> Sequence() => new(_definition);

		public GenericExtent<T1, T2, T3, T4, Func<T>> Delegate() => new(_definition);

		public GenericExtent<T1, T2, T3, T4, ICondition<T>> Condition() => new(_definition);

		public GenericExtent<T1, T2, T3, T4, IResult<T>> Result() => new(_definition);

		public GenericExtent<T1, T2, T3, T4, ICommand<T>> Command() => new(_definition);
	}
}