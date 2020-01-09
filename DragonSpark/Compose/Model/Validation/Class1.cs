using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class EnsureSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public EnsureSelectionContext(ISelect<TIn, TOut> select) => _subject = select;

		public ConditionalInputSelectionContext<TIn, TOut> Input
			=> new ConditionalInputSelectionContext<TIn, TOut>(_subject);

		public ConditionalOutputSelectionContext<TIn, TOut> Output
			=> new ConditionalOutputSelectionContext<TIn, TOut>(_subject);
	}

	public sealed class ConditionalInputSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public ConditionalInputSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public AssignedInputConditionSelectionContext<TIn, TOut> IsAssigned
			=> new AssignedInputConditionSelectionContext<TIn, TOut>(_subject);

		public InputConditionSelectionContext<TIn, TOut> IsOf<T>() => Is(IsOf<TIn, T>.Default);

		public InputConditionSelectionContext<TIn, TOut> Is(ISelect<TIn, bool> condition) => Is(condition.Get);

		public InputConditionSelectionContext<TIn, TOut> Is(Func<TIn, bool> condition)
			=> new InputConditionSelectionContext<TIn, TOut>(_subject, condition);
	}

	public sealed class AssignedInputConditionSelectionContext<TIn, TOut>
	{
		public AssignedInputConditionSelectionContext(ISelect<TIn, TOut> otherwise)
			: this(new AssignedInputOtherwiseContext<TIn, TOut>(otherwise)) {}

		public AssignedInputConditionSelectionContext(AssignedInputOtherwiseContext<TIn, TOut> otherwise)
			=> Otherwise = otherwise;

		public AssignedInputOtherwiseContext<TIn, TOut> Otherwise { get; }
	}

	public sealed class InputConditionSelectionContext<TIn, TOut>
	{
		public InputConditionSelectionContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
			: this(new OtherwiseThrowInputContext<TIn, TOut>(subject, condition)) {}

		public InputConditionSelectionContext(OtherwiseThrowInputContext<TIn, TOut> otherwise)
			=> Otherwise = otherwise;

		public OtherwiseThrowInputContext<TIn, TOut> Otherwise { get; }
	}

	public sealed class AssignedInputOtherwiseContext<TIn, TOut> : InputOtherwiseContext<TIn, TOut>
	{
		public AssignedInputOtherwiseContext(ISelect<TIn, TOut> subject) : base(subject, Is.Assigned<TIn>().Get) {}

		public ConditionalSelector<TIn, TOut> Throw() => Throw(AssignedArgumentMessage.Default);

		public ConditionalSelector<TIn, TOut> Throw(IResult<string> message)
			=> Throw(message.Then().Accept<Type>().Return());

		public ConditionalSelector<TIn, TOut> Throw(ISelect<Type, string> message)
			=> Throw(message.Then().Bind<TIn>().Get());

		public ConditionalSelector<TIn, TOut> Throw(ISelect<TIn, string> message)
			=> new AssignedThrowContext<TIn, TOut>(this).WithMessage(message);
	}

	public class OtherwiseThrowInputContext<TIn, TOut> : InputOtherwiseContext<TIn, TOut>
	{
		public OtherwiseThrowInputContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
			: base(subject, condition) {}

		public InputOtherwiseThrowContext<TIn, TOut> Throw<TException>() where TException : Exception
			=> new InputOtherwiseThrowContext<TIn, TOut>(this, Select<TException>.Default.Get);

		sealed class Select<T> : ISelect<ISelect<TIn, string>, ICommand<TIn>> where T : Exception
		{
			public static Select<T> Default { get; } = new Select<T>();

			Select() {}

			public ICommand<TIn> Get(ISelect<TIn, string> parameter) => new Guard<TIn, T>(Is.Always<TIn>(), parameter);
		}
	}

	public class InputOtherwiseContext<TIn, TOut>
	{
		readonly Func<TIn, bool>    _condition;
		readonly ISelect<TIn, TOut> _subject;

		public InputOtherwiseContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
		{
			_subject   = subject;
			_condition = condition;
		}

		public ConditionalSelector<TIn, TOut> UseDefault() => Use(Start.A.Selection<TIn>().By.Default<TOut>());

		public ConditionalSelector<TIn, TOut> Use(ISelect<TIn, TOut> instead) => Use(instead.Get);

		public ConditionalSelector<TIn, TOut> Use(Func<TIn, TOut> instead)
			=> new Conditional<TIn, TOut>(_condition, _subject.Get, instead).Then();

		public ConditionalSelector<TIn, TOut> Use(ICommand<TIn> instead) => Use(instead.Then()
		                                                                               .ToConfiguration()
		                                                                               .Default<TOut>());
	}

	sealed class AssignedThrowContext<TIn, TOut> : InputOtherwiseThrowContext<TIn, TOut>
	{
		public AssignedThrowContext(InputOtherwiseContext<TIn, TOut> input)
			: base(input, x => new AssignedEntryGuard<TIn>(x)) {}
	}

	public class InputOtherwiseThrowContext<TIn, TOut>
	{
		readonly Func<ISelect<TIn, string>, ICommand<TIn>> _guard;
		readonly InputOtherwiseContext<TIn, TOut>          _input;

		public InputOtherwiseThrowContext(InputOtherwiseContext<TIn, TOut> input,
		                                  Func<ISelect<TIn, string>, ICommand<TIn>> guard)
		{
			_input = input;
			_guard = guard;
		}

		public Selector<TIn, TOut> WithMessage(string message) => WithMessage(Start.A.Result(message));

		public Selector<TIn, TOut> WithMessage(IResult<string> message) => WithMessage(message.Then().Accept<TIn>().Return());

		public ConditionalSelector<TIn, TOut> WithMessage(ISelect<TIn, string> message) => _input.Use(_guard(message));
	}

	public class OutputOtherwiseThrowContext<TIn, TOut>
	{
		readonly Func<ISelect<TIn, string>, ICommand<TIn>> _guard;
		readonly OutputOtherwiseContext<TIn, TOut>         _input;

		public OutputOtherwiseThrowContext(OutputOtherwiseContext<TIn, TOut> input,
		                                   Func<ISelect<TIn, string>, ICommand<TIn>> guard)
		{
			_input = input;
			_guard = guard;
		}

		public Selector<TIn, TOut> WithMessage(string message) => WithMessage(Start.A.Result(message));

		public Selector<TIn, TOut> WithMessage(IResult<string> message) => WithMessage(message.Then().Accept<TIn>().Return());

		public Selector<TIn, TOut> WithMessage(ISelect<TIn, string> message) => _input.Use(_guard(message));
	}

	public sealed class ConditionalOutputSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public ConditionalOutputSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public AssignedOutputGuardContext<TIn, TOut> IsAssigned => new AssignedOutputGuardContext<TIn, TOut>(_subject);

		public OutputConditionSelectionContext<TIn, TOut> IsOf<T>() => Is(IsOf<TOut, T>.Default);

		public OutputConditionSelectionContext<TIn, TOut> Is(ISelect<TOut, bool> condition) => Is(condition.Get);
		
		public OutputConditionSelectionContext<TIn, TOut> Is(Func<TOut, bool> condition)
			=> new OutputConditionSelectionContext<TIn, TOut>(_subject, condition);
	}

	public sealed class OutputConditionSelectionContext<TIn, TOut>
	{
		public OutputConditionSelectionContext(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
			: this(new OtherwiseThrowOutputContext<TIn, TOut>(subject, condition)) {}

		public OutputConditionSelectionContext(OtherwiseThrowOutputContext<TIn, TOut> otherwise)
			=> Otherwise = otherwise;

		public OtherwiseThrowOutputContext<TIn, TOut> Otherwise { get; }
	}

	public class OtherwiseThrowOutputContext<TIn, TOut> : OutputOtherwiseContext<TIn, TOut>
	{
		public OtherwiseThrowOutputContext(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
			: base(subject, condition) {}

		public OutputOtherwiseThrowContext<TIn, TOut> Throw<TException>() where TException : Exception
			=> new OutputOtherwiseThrowContext<TIn, TOut>(this, Select<TException>.Default.Get);

		sealed class Select<T> : ISelect<ISelect<TIn, string>, ICommand<TIn>> where T : Exception
		{
			public static Select<T> Default { get; } = new Select<T>();

			Select() {}

			public ICommand<TIn> Get(ISelect<TIn, string> parameter) => new Guard<TIn, T>(Is.Always<TIn>(), parameter);
		}
	}

	public class OutputOtherwiseContext<TIn, TOut>
	{
		readonly Func<TOut, bool>   _condition;
		readonly ISelect<TIn, TOut> _subject;

		public OutputOtherwiseContext(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
		{
			_subject   = subject;
			_condition = condition;
		}

		public Selector<TIn, TOut> UseDefault() => Use(Start.A.Selection<TIn>().By.Default<TOut>());

		public Selector<TIn, TOut> Use(ISelect<TIn, TOut> instead) => Use(instead.Get);

		public Selector<TIn, TOut> Use(Func<TIn, TOut> instead)
			=> new ValidatedResult<TIn, TOut>(_condition, _subject.Get, instead).Then();

		public Selector<TIn, TOut> Use(ICommand<TIn> instead) => Use(instead.Then()
		                                                                    .ToConfiguration()
		                                                                    .Default<TOut>());
	}

	public sealed class AssignedOutputGuardContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public AssignedOutputGuardContext(ISelect<TIn, TOut> select) => _subject = select;

		public Selector<TIn, TOut> OrThrow() => OrThrow(AssignedResultMessage.Default);

		public Selector<TIn, TOut> OrThrow(IResult<string> message) => OrThrow(message.Then().Accept<Type>().Return());

		public Selector<TIn, TOut> OrThrow(ISelect<Type, string> message)
			=> Start.A.Selection<TIn>()
			        .AndOf<Type>()
			        .By.Cast.Or.Return(A.Type<TOut>())
			        .Then()
			        .Terminate(Start.A.Guard<InvalidOperationException>()
			                        .Displaying(message)
			                        .When(Is.Always()))
			        .ToConfiguration()
			        .Default<TOut>()
			        .Unless.Using(_subject)
			        .ResultsInAssigned();
	}
}