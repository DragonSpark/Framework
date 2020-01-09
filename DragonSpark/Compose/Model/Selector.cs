using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Invocation;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model
{
	public class Selector<T> : Selector<None, T>
	{
		public static implicit operator Func<T>(Selector<T> instance) => instance.Get().Get;

		public Selector(ISelect<None, T> subject) : base(subject) {}
	}

	public class Selector<TIn, TOut> : IResult<ISelect<TIn, TOut>>, IActivateUsing<ISelect<TIn, TOut>>
	{
		public static implicit operator Func<TIn, TOut>(Selector<TIn, TOut> instance) => instance.Get().ToDelegate();

		readonly ISelect<TIn, TOut> _subject;

		public Selector(ISelect<TIn, TOut> subject) => _subject = subject;

		public ConditionalSelectionContext<TIn, TOut> Ensure => new ConditionalSelectionContext<TIn, TOut>(_subject);

		public UnlessContext<TIn, TOut> Unless => new UnlessContext<TIn, TOut>(_subject);

		public TypeSelector<TIn> Type() => new TypeSelector<TIn>(_subject.Select(InstanceType<TOut>.Default));

		public Selector<TIn, TTo> StoredActivation<TTo>() where TTo : IActivateUsing<TOut>
			=> Select(Activations<TOut, TTo>.Default);

		public Selector<TIn, TTo> Select<TTo>(ISelect<TOut, TTo> select) => Select(select.Get);

		public Selector<TIn, TTo> Select<TTo>(Func<TOut, TTo> select)
			=> new Selection<TIn, TOut, TTo>(Get().Get, select).Then();

		public Selector<TIn, TOut> Configure(IAssign<TIn, TOut> configuration)
			=> new Configuration<TIn, TOut>(_subject, configuration).Then();

		public Selector<TIn, TOut> Configure(ICommand<(TIn, TOut)> configuration)
			=> new Configuration<TIn, TOut>(_subject.Get, configuration.Execute).Then();

		public Selector<TIn, TOut> Configure<TOther>(IAssign<TIn, TOther> configuration)
			=> new Configuration<TIn, TOut, TOther>(_subject, configuration).Then();

		public Selector<TIn, Array<TOut>> Result() => Select(x => x.Yield().Result());

		public Selector<TIn, TOut> EnsureAssignedOrDefault() => OrDefault(Is.Assigned<TIn>().Get);

		public Selector<TIn, TOut> OrDefault(ISelect<TIn, bool> use) => OrDefault(use.Get);

		public Selector<TIn, TOut> OrDefault(Func<TIn, bool> use)
			=> OrDefault(use, Start.A.Selection<TIn>().By.Default<TOut>());

		public Selector<TIn, TOut> OrDefault(Func<TIn, bool> use, ISelect<TIn, TOut> @default)
			=> Ensure.Input.Is(use)
			         .Otherwise.Use(@default);

		public Selector<TIn, TTo> Default<TTo>() => Select(Default<TOut, TTo>.Instance);

		public Selector<TIn, TTo> Cast<TTo>() => Select(CastOrDefault<TOut, TTo>.Default);

		public Selector<TIn, TTo> CastForResult<TTo>() => Select(ResultAwareCast<TOut, TTo>.Default);

		public Selector<TIn, (TIn, TOut)> Introduce() => new Introduce<TIn, TOut>(_subject).Then();

		public Selector<TIn, TOut> OnceStriped() => OncePerParameter<TIn, TOut>.Default.Get(_subject).Then();

		public Selector<TIn, TOut> OnlyOnce() => OnlyOnceAlteration<TIn, TOut>.Default.Get(_subject).Then();

		public Selector<TIn, TOut> Protect() => ProtectAlteration<TIn, TOut>.Default.Get(_subject).Then();

		public Selector<TIn, TOut> Stripe() => StripedAlteration<TIn, TOut>.Default.Get(_subject).Then();

		public Selector<TIn, TOut> Try<TException>() where TException : Exception
			=> new Try<TException, TIn, TOut>(Get().Get).Then();

		public CommandContext<TIn> Terminate() => new InvokeParameterCommand<TIn, TOut>(Get().Get).Then();

		public CommandContext<TIn> Terminate(ICommand<TOut> command) => Terminate(command.Execute);

		public CommandContext<TIn> Terminate(System.Action<TOut> command)
			=> new SelectedParameterCommand<TIn, TOut>(command, _subject.Get).Then();

		public ISelect<TIn, TOut> Get() => _subject;
	}

	public sealed class UnlessUsingContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;
		readonly ISelect<TIn, TOut> _other;

		public UnlessUsingContext(ISelect<TIn, TOut> subject, ISelect<TIn, TOut> other)
		{
			_subject = subject;
			_other   = other;
		}

		public Selector<TIn, TOut> IsOf<T>() => Results(IsOf<TOut, T>.Default.Get);

		public Selector<TIn, TOut> ResultsInAssigned() => Results(Is.Assigned<TOut>().Get);

		public Selector<TIn, TOut> Results(Func<TOut, bool> @in)
			=> new ValidatedResult<TIn, TOut>(@in, _other.Get, _subject.Get).Then();
	}

	public sealed class UnlessResultContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;
		readonly Func<TIn, bool>    _condition;

		public UnlessResultContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
		{
			_subject   = subject;
			_condition = condition;
		}

		public ConditionalSelector<TIn, TOut> ThenUse(ISelect<TIn, TOut> instead) => ThenUse(instead.Get);

		public ConditionalSelector<TIn, TOut> ThenUse(Func<TIn, TOut> instead)
			=> new Conditional<TIn, TOut>(_condition, instead, _subject.Get).Then();
	}

	public sealed class UnlessInputContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public UnlessInputContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public Selector<TIn, TOut> IsOf<TOther>(ISelect<TOther, TOut> other) => IsOf(other.ToDelegate());

		public Selector<TIn, TOut> IsOf<TOther>(Func<TOther, TOut> other)
			=> IsOf<TOther>().ThenUse(CastOrThrow<TIn, TOther>.Default.Select(other));

		public UnlessResultContext<TIn, TOut> IsOf<T>() => Is(IsOf<TIn, T>.Default);

		public UnlessResultContext<TIn, TOut> IsUnassigned() => Is(Compose.Is.Assigned<TIn>().Then().Inverse());

		public UnlessResultContext<TIn, TOut> Is(ISelect<TIn, bool> condition) => Is(condition.Get);

		public UnlessResultContext<TIn, TOut> Is(Func<TIn, bool> condition)
			=> new UnlessResultContext<TIn, TOut>(_subject, condition);
	}

	public sealed class UnlessContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public UnlessContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public UnlessInputContext<TIn, TOut> Input => new UnlessInputContext<TIn, TOut>(_subject);

		public UnlessUsingContext<TIn, TOut> Using(ISelect<TIn, TOut> instead)
			=> new UnlessUsingContext<TIn, TOut>(_subject, instead);

		public ConditionalSelector<TIn, TTo> UsingWhen<TTo>(IConditional<TOut, TTo> select)
			=> new Conditional<TIn, TTo>(_subject.Select(select.Condition).Get, _subject.Select(select).Get).Then();
	}

	public sealed class ConditionalSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public ConditionalSelectionContext(ISelect<TIn, TOut> select) => _subject = select;

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
		readonly Func<TIn, bool> _condition;

		public OtherwiseThrowInputContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
			: base(subject, condition) => _condition = condition;

		public OtherwiseThrowContext<TIn, TOut> Throw<TException>() where TException : Exception
			=> new OtherwiseThrowContext<TIn, TOut>(this, new Select<TException>(_condition).Get);

		sealed class Select<T> : ISelect<ISelect<TIn, string>, ICommand<TIn>> where T : Exception
		{
			readonly Func<TIn, bool> _condition;

			public Select(Func<TIn, bool> condition) => _condition = condition;

			public ICommand<TIn> Get(ISelect<TIn, string> parameter) => new Guard<TIn, T>(_condition, parameter);
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

	sealed class AssignedThrowContext<TIn, TOut> : OtherwiseThrowContext<TIn, TOut>
	{
		public AssignedThrowContext(InputOtherwiseContext<TIn, TOut> input)
			: base(input, x => new AssignedEntryGuard<TIn>(x)) {}
	}

	public class OtherwiseThrowContext<TIn, TOut>
	{
		readonly Func<ISelect<TIn, string>, ICommand<TIn>> _guard;
		readonly InputOtherwiseContext<TIn, TOut>          _input;

		public OtherwiseThrowContext(InputOtherwiseContext<TIn, TOut> input,
		                             Func<ISelect<TIn, string>, ICommand<TIn>> guard)
		{
			_input = input;
			_guard = guard;
		}

		public ConditionalSelector<TIn, TOut> WithMessage(ISelect<TIn, string> message) => _input.Use(_guard(message));
	}

	public sealed class ConditionalSelector<TIn, TOut> : Selector<TIn, TOut>, IResult<IConditional<TIn, TOut>>
	{
		public static implicit operator Func<TIn, TOut>(ConditionalSelector<TIn, TOut> instance) => instance.Get().Get;

		readonly IConditional<TIn, TOut> _subject;

		public ConditionalSelector(IConditional<TIn, TOut> subject) : base(subject) => _subject = subject;

		public new IConditional<TIn, TOut> Get() => _subject;
	}

	public sealed class ConditionalOutputSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public ConditionalOutputSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public AssignedOutputGuardContext<TIn, TOut> IsAssigned => new AssignedOutputGuardContext<TIn, TOut>(_subject);
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