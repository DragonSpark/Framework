using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Invocation;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Model.Selection.Adapters
{
	public sealed class MessageSelector : Selector<Type, string>
	{
		public MessageSelector(ISelect<Type, string> subject) : base(subject) {}

		/*public Selector<T, T> Guard<T>()
			=> Start.A.Selection<T>().By.Self.Then().Validate().EnsureAssigned(For<T>().Get());*/

		/*public Selector<Type, T> Bind<T>(ISelect<Type, T> subject) => Bind(A.Type<T>());

		public Selector<Type, string> Bind<T>() => Bind(A.Type<T>());

		public Selector<Type, string> Bind(Type type)
			=> new Selector<Type, string>(Get().In(type).ToSelect(Start.An.Extent<Type>()));
		*/

		public Selector<Type, string> For<T>() => Get().In(A.Type<T>()).ToSelect(Start.An.Extent<Type>()).Then();

		public Selector<T, string> Bind<T>() => Get().In(A.Type<T>()).ToSelect(Start.An.Extent<T>()).Then();
	}

	public class Selector<_, T> : IResult<ISelect<_, T>>, IActivateUsing<ISelect<_, T>>
	{
		public static implicit operator Func<_, T>(Selector<_, T> instance) => instance.Get().ToDelegate();

		readonly ISelect<_, T> _subject;

		public Selector(ISelect<_, T> subject) => _subject = subject;

		public ISelect<_, T> Get() => _subject;

		public TypeSelector<_> Type() => new TypeSelector<_>(_subject.Select(InstanceType<T>.Default));

		public Selector<_, TTo> Select<TTo>() where TTo : IActivateUsing<T> => Select(Activations<T, TTo>.Default);

		public Selector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public Selector<_, TTo> Select<TTo>(Func<T, TTo> select)
			=> new Selector<_, TTo>(new Selection<_, T, TTo>(Get().Get, select));

		public Selector<_, T> Configure(IAssign<_, T> configuration)
			=> new Selector<_, T>(new Configuration<_, T>(_subject, configuration));

		public Selector<_, T> Configure(ICommand<(_, T)> configuration)
			=> new Selector<_, T>(new Configuration<_, T>(_subject.Get, configuration.Execute));

		public Selector<_, T> Configure<TOther>(IAssign<_, TOther> configuration)
			=> new Selector<_, T>(new Configuration<_, T, TOther>(_subject, configuration));

		public Selector<_, Array<T>> Result() => Select(x => x.Yield().Result());

		public Selector<_, T> Assigned() => Get().If(IsAssigned<_>.Default).Then();

		public Selector<_, TTo> Cast<TTo>() => Select(CastOrDefault<T, TTo>.Default);

		public Selector<_, TTo> CastForResult<TTo>() => Select(ResultAwareCast<T, TTo>.Default);

		public Selector<_, (_, T)> Introduce() => new Selector<_, (_, T)>(new Introduce<_, T>(_subject));

		public Selector<_, T> OnceStriped() => new Selector<_, T>(OncePerParameter<_, T>.Default.Get(_subject));

		public Selector<_, T> OnlyOnce() => new Selector<_, T>(OnlyOnceAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Protect() => new Selector<_, T>(ProtectAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Stripe() => new Selector<_, T>(StripedAlteration<_, T>.Default.Get(_subject));

		public GuardContext<_, T> Ensure => new GuardContext<_, T>(_subject);

		public Selector<_, T> Try<TException>() where TException : Exception
			=> new Selector<_, T>(new Try<TException, _, T>(Get().Get));

		public CommandSelector<_> Terminate() => new CommandSelector<_>(new InvokeParameterCommand<_, T>(Get().Get));

		public CommandSelector<_> Terminate(ICommand<T> command) => Terminate(command.Execute);

		public CommandSelector<_> Terminate(System.Action<T> command)
			=> new CommandSelector<_>(new SelectedParameterCommand<_, T>(command, _subject.Get));
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

	public sealed class GuardModelContext
	{
		public static GuardModelContext Default { get; } = new GuardModelContext();

		GuardModelContext() {}

		/*public AssignedGuardModelContext Assigned() => Assigned(AssignedResultMessage.Default);

		public AssignedGuardModelContext Assigned(ISelect<Type, string> message)
			=> new AssignedGuardModelContext(message);*/
	}

	/*public sealed class AssignedGuardModelContext
	{
		readonly ISelect<Type, string> _message;

		public AssignedGuardModelContext(ISelect<Type, string> message) => _message = message;

		public ISelect<T, string> For<T>() => _message.In(A.Type<T>()).ToSelect(Start.An.Extent<T>());

		public ISelect<Type, string> Bind<T>() => _message.In(A.Type<T>()).ToSelect(Start.An.Extent<Type>());

		public ICommand<T> Command<T>() => Command(For<T>());

		public ICommand<T> Command<T>(ISelect<T, string> message) => new AssignedResultGuard<T>(message);
	}*/

	/*public sealed class AssignedGuardContext<T, TOut>
	{
		readonly Func<ICommand<T>, TOut> _out;

		public AssignedGuardContext(Func<ICommand<T>, TOut> @out) => _out = @out;


	}*/

	/*public sealed class GuardEntryContext<T, _>
	{
		readonly ISelect<T, _> _subject;

		public GuardEntryContext(ISelect<T, _> select) => _subject = select;

		public AssignedGuardContext<T, Selector<T, _>> IsAssigned => new AssignedGuardContext<T, Selector<T, _>>(Using);

		public Selector<T, _> Using(ICommand<T> guard)
			=> new Selector<T, _>(guard.Then().ToConfiguration().Select(_subject).Get());
	}*/

	public sealed class AssignedEntryGuardContext<T, _>
	{
		readonly ISelect<T, _> _subject;

		public AssignedEntryGuardContext(ISelect<T, _> select) => _subject = select;

		public Selector<T, _> OrThrow() => OrThrow(AssignedResultMessage.Default);

		public Selector<T, _> OrThrow(ISelect<Type, string> message) => OrThrow(message.Then().Bind<T>().Get());

		public Selector<T, _> OrThrow(ISelect<T, string> message) => Using(new AssignedResultGuard<T>(message));

		public Selector<T, _> Using(ICommand<T> guard)
			=> guard.Then().ToConfiguration().Select(_subject).Get().Then();
	}

	public sealed class AssignedExitGuardContext<_, T>
	{
		readonly ISelect<_, T> _subject;

		public AssignedExitGuardContext(ISelect<_, T> select) => _subject = select;

		public Selector<_, T> OrThrow() => OrThrow(AssignedResultMessage.Default);

		public Selector<_, T> OrThrow(ISelect<Type, string> message) => OrThrow(message.Then().Bind<T>().Get());

		public Selector<_, T> OrThrow(ISelect<T, string> message) => Using(new AssignedResultGuard<T>(message));

		public Selector<_, T> Using(ICommand<T> guard)
			=> _subject.Then().Select(guard.Then().ToConfiguration().Get()).Get().Then();
	}
}