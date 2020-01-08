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

	public class Selector<_, T> : IResult<ISelect<_, T>>, IActivateUsing<ISelect<_, T>>
	{
		public static implicit operator Func<_, T>(Selector<_, T> instance) => instance.Get().ToDelegate();

		readonly ISelect<_, T> _subject;

		public Selector(ISelect<_, T> subject) => _subject = subject;

		public GuardContext<_, T> Ensure => new GuardContext<_, T>(_subject);

		public ValidatedSelectionContext<_, T> Otherwise => new ValidatedSelectionContext<_, T>(_subject);

		public ISelect<_, T> Get() => _subject;

		public TypeSelector<_> Type() => new TypeSelector<_>(_subject.Select(InstanceType<T>.Default));

		/**/

		public Selector<_, TTo> Select<TTo>() where TTo : IActivateUsing<T> => Select(Activations<T, TTo>.Default);

		public Selector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public Selector<_, TTo> Select<TTo>(Func<T, TTo> select) => new Selection<_, T, TTo>(Get().Get, select).Then();

		public Selector<_, T> Configure(IAssign<_, T> configuration)
			=> new Configuration<_, T>(_subject, configuration).Then();

		public Selector<_, T> Configure(ICommand<(_, T)> configuration)
			=> new Configuration<_, T>(_subject.Get, configuration.Execute).Then();

		public Selector<_, T> Configure<TOther>(IAssign<_, TOther> configuration)
			=> new Configuration<_, T, TOther>(_subject, configuration).Then();

		public Selector<_, Array<T>> Result() => Select(x => x.Yield().Result());

		public Selector<_, T> Assigned() => Get().If(Is.Assigned<_>()).Then();

		public Selector<_, TTo> Cast<TTo>() => Select(CastOrDefault<T, TTo>.Default);

		public Selector<_, TTo> CastForResult<TTo>() => Select(ResultAwareCast<T, TTo>.Default);

		public Selector<_, (_, T)> Introduce() => new Introduce<_, T>(_subject).Then();

		public Selector<_, T> OnceStriped() => OncePerParameter<_, T>.Default.Get(_subject).Then();

		public Selector<_, T> OnlyOnce() => OnlyOnceAlteration<_, T>.Default.Get(_subject).Then();

		public Selector<_, T> Protect() => ProtectAlteration<_, T>.Default.Get(_subject).Then();

		public Selector<_, T> Stripe() => StripedAlteration<_, T>.Default.Get(_subject).Then();

		public Selector<_, T> Try<TException>() where TException : Exception
			=> new Try<TException, _, T>(Get().Get).Then();

		public CommandSelector<_> Terminate() => new InvokeParameterCommand<_, T>(Get().Get).Then();

		public CommandSelector<_> Terminate(ICommand<T> command) => Terminate(command.Execute);

		public CommandSelector<_> Terminate(System.Action<T> command)
			=> new SelectedParameterCommand<_, T>(command, _subject.Get).Then();
	}

	public sealed class ValidatedSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public ValidatedSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public Selector<TIn, TOut> UseWhenAssigned(Func<TOut> assigned)
			=> UseWhenAssigned(Start.A.Result(assigned).Then().Accept<TIn>());

		public Selector<TIn, TOut> UseWhenAssigned(ISelect<TIn, TOut> assigned)
			=> UseWhenAssigned(assigned.ToDelegate());

		Selector<TIn, TOut> UseWhenAssigned(Func<TIn, TOut> assigned)
			=> new ValidatedResult<TIn, TOut>(Is.Assigned<TOut>().Get, assigned, _subject.Get).Then();

		public UseValidatedSelectionContext<TIn, TOut> Use(ISelect<TIn, TOut> other)
			=> new UseValidatedSelectionContext<TIn, TOut>(_subject, other);

		public IConditional<TIn, TOut> Use<TOther>(ISelect<TOther, TOut> other)
			=> new UseValidatedSelectionContext<TIn, TOut>(_subject, CastOrThrow<TIn, TOther>.Default.Select(other))
				.WhenIsOf<TOther>();

		public IConditional<TIn, TTo> Use<TTo>(IConditional<TOut, TTo> select)
			=> new Conditional<TIn, TTo>(_subject.Select(select.Condition).Get, _subject.Select(select.Get).Get);
	}

	public sealed class UseValidatedSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;
		readonly ISelect<TIn, TOut> _other;

		public UseValidatedSelectionContext(ISelect<TIn, TOut> subject, ISelect<TIn, TOut> other)
		{
			_subject = subject;
			_other   = other;
		}

		public IConditional<TIn, TOut> WhenIsOf<T>() => When(IsOf<TIn, T>.Default);

		public IConditional<TIn, TOut> WhenAssigned() => When(Is.Assigned<TIn>());

		public IConditional<TIn, TOut> When(Func<TIn, bool> condition) => When(Start.A.Condition(condition));

		public IConditional<TIn, TOut> When(ICondition<TIn> condition)
			=> new Conditional<TIn, TOut>(condition.Get, _other.Get, _subject.Get);
	}

	public sealed class GuardContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public GuardContext(ISelect<TIn, TOut> select) => _subject = select;

		public AssignedGuardContext<TIn, TOut> Assigned => new AssignedGuardContext<TIn, TOut>(_subject);
	}

	public sealed class AssignedGuardContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public AssignedGuardContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public AssignedEntryGuardContext<TIn, TOut> Entry => new AssignedEntryGuardContext<TIn, TOut>(_subject);

		public AssignedExitGuardContext<TIn, TOut> Exit => new AssignedExitGuardContext<TIn, TOut>(_subject);
	}

	public sealed class GuardModelContext<TException> where TException : Exception
	{
		public static GuardModelContext<TException> Default { get; } = new GuardModelContext<TException>();

		GuardModelContext() {}

		public GuardThrowContext<T, TException> Displaying<T>(ISelect<T, string> message)
			=> new GuardThrowContext<T, TException>(message);
	}

	public sealed class GuardThrowContext<T, TException> where TException : Exception
	{
		readonly ISelect<T, string> _message;

		public GuardThrowContext(ISelect<T, string> message) => _message = message;

		public CommandSelector<T> WhenUnassigned() => When(Is.Assigned<T>().Then().Inverse().Out());

		public CommandSelector<T> When(ICondition<T> condition) => new Guard<T, TException>(condition, _message).Then();
	}

	public sealed class AssignedEntryGuardContext<T, _>
	{
		readonly ISelect<T, _> _subject;

		public AssignedEntryGuardContext(ISelect<T, _> select) => _subject = select;

		public Selector<T, _> OrThrow() => OrThrow(AssignedResultMessage.Default);

		public Selector<T, _> OrThrow(ISelect<Type, string> message) => OrThrow(message.Then().Bind<T>().Get());

		public Selector<T, _> OrThrow(ISelect<T, string> message)
			=> new AssignedEntryGuard<T>(message).Then().ToConfiguration().Select(_subject);
	}

	public sealed class AssignedExitGuardContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public AssignedExitGuardContext(ISelect<TIn, TOut> select) => _subject = select;

		public Selector<TIn, TOut> OrThrow() => OrThrow(AssignedResultMessage.Default);

		public Selector<TIn, TOut> OrThrow(IResult<string> message) => OrThrow(message.Then().Accept<Type>().Return());

		public Selector<TIn, TOut> OrThrow(ISelect<Type, string> message)
			// TODO: Convert to Unless
			/*_subject.Unless(Is.Assigned<T>().Then().Inverse().Get(),
						                   guard.Then().ToConfiguration().Select(x => default(T)).Get())
						           */
			=> new ValidatedResult<TIn, TOut>(Is.Assigned<TOut>(),
			                                  _subject,
			                                  Start.A.Selection<TIn>()
			                                       .AndOf<Type>()
			                                       .By.Cast.Or.Return(A.Type<TOut>())
			                                       .Then()
			                                       .Terminate(Start.A.Guard<InvalidOperationException>()
			                                                       .Displaying(message)
			                                                       .When(Is.Always()))
			                                       .ToConfiguration()
			                                       .Select(x => default(TOut))
			                                       .Get())
				.Then();
	}
}