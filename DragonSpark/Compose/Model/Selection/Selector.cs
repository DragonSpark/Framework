using DragonSpark.Compose.Model.Commands;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Sequences;
using DragonSpark.Compose.Model.Validation;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Invocation;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model.Selection
{
	public class Selector<T> : Selector<None, T>
	{
		public static implicit operator Func<T>(Selector<T> instance) => instance.Get().Get;

		public Selector(ISelect<None, T> subject) : base(subject) {}
	}

	public class Selector<TIn, TOut> : IResult<ISelect<TIn, TOut>>, IActivateUsing<ISelect<TIn, TOut>>
	{
		public static implicit operator Func<TIn, TOut>(Selector<TIn, TOut> instance) => instance.Get().Get;


		readonly ISelect<TIn, TOut> _subject;

		public Selector(ISelect<TIn, TOut> subject) => _subject = subject;

		public EnsureSelectionContext<TIn, TOut> Ensure => new EnsureSelectionContext<TIn, TOut>(_subject);

		public UnlessContext<TIn, TOut> Unless => new UnlessContext<TIn, TOut>(_subject);

		public TypeSelector<TIn> Type() => new TypeSelector<TIn>(_subject.Select(InstanceType<TOut>.Default));

		public Selector<TIn, TTo> StoredActivation<TTo>() where TTo : IActivateUsing<TOut>
			=> Select(Activations<TOut, TTo>.Default);

		public ResultContext<TOut> Bind(TIn parameter)
			=> new FixedSelection<TIn, TOut>(_subject, parameter).Then();

		public ResultContext<TOut> Bind(IResult<TIn> parameter) => Bind(parameter.Get);

		public ResultContext<TOut> Bind(Func<TIn> parameter)
			=> new DelegatedSelection<TIn, TOut>(_subject.Get, parameter).Then();

		public Selector<TIn, TTo> Select<TTo>(Selector<TOut, TTo> select) => Select(select.Get());

		public Selector<TIn, TTo> Select<TTo>(ISelect<TOut, TTo> select) => Select(select.Get);

		public Selector<TIn, TTo> Select<TTo>(Func<TOut, TTo> select)
			=> new Selection<TIn, TOut, TTo>(Get().Get, select).Then();

		public Selector<TIn, TOut> Configure(IAssign<TIn, TOut> configure)
			=> Configure(configure.Assign);

		public Selector<TIn, TOut> Configure(ICommand<(TIn, TOut)> configuration) => Configure(configuration.Execute);

		public Selector<TIn, TOut> Configure(Action<TIn, TOut> configure)
			=> new DragonSpark.Model.Selection.Configure<TIn, TOut>(_subject.Get, configure).Then();

		public Selector<TIn, TOut> Configure(System.Action<TOut> configure)
			=> new ConfigureOutput<TIn, TOut>(_subject.Get, configure).Then();

		public SequenceSelector<TIn, TOut> Yield() => new SequenceSelector<TIn, TOut>(Select(x => x.Yield()).Get());

		public Selector<TIn, TOut> EnsureAssignedOrDefault() => OrDefault(Is.Assigned<TIn>());

		public Selector<TIn, TOut> OrDefault(ISelect<TIn, bool> use) => OrDefault(use.Get);

		public Selector<TIn, TOut> OrDefault(Func<TIn, bool> use)
			=> OrDefault(use, Start.A.Selection<TIn>().By.Default<TOut>());

		public Selector<TIn, TOut> OrDefault(Func<TIn, bool> use, Func<TIn, TOut> @default)
			=> Ensure.Input.Is(use)
			         .Otherwise.Use(@default);

		public Selector<TIn, TTo> Default<TTo>() => Select(Default<TOut, TTo>.Instance);

		public Selector<TIn, TTo> Cast<TTo>() => Select(CastOrDefault<TOut, TTo>.Default);

		public Selector<TIn, TTo> CastForResult<TTo>() => Select(ResultAwareCast<TOut, TTo>.Default);

		public Selector<TIn, (TIn, TOut)> Introduce() => new Introduce<TIn, TOut>(_subject).Then();

		public Selector<TIn, TOut> OnlyOnce() => OnlyOnceAlteration<TIn, TOut>.Default.Get(_subject).Then();

		public Selector<TIn, TOut> Protect() => ProtectAlteration<TIn, TOut>.Default.Get(_subject).Then();

#pragma warning disable 8714
		public Selector<TIn, TOut> Stripe() => StripedAlteration<TIn, TOut>.Default.Get(_subject).Then();

		public Selector<TIn, TOut> OnceStriped() => OncePerParameter<TIn, TOut>.Default.Get(_subject).Then();

		public TableSelector<TIn, T> Table<T>() => Table(Tables<TOut, T>.Default.Get(_ => default!));

		public TableSelector<TIn, T> Table<T>(ITable<TOut, T> table)
			=> new DelegatedTable<TIn, T>(Select(table.Condition.Get), Terminate(table), Select(table.Get),
			                              Select(table.Remove)).Then();
#pragma warning restore 8714

		public Selector<TIn, TOut> Try<TException>() where TException : Exception
			=> new Try<TException, TIn, TOut>(Get().Get).Then();

		public CommandContext<TIn> Terminate() => new InvokeParameterCommand<TIn, TOut>(this).Then();

		public CommandContext<TIn> Terminate(ICommand<TOut> command) => Terminate(command.Execute);

		public CommandContext<TIn> Terminate(System.Action<TOut> command)
			=> new SelectedParameterCommand<TIn, TOut>(command, _subject.Get).Then();

		public CommandContext<TIn> Terminate<_>(Func<TOut, _> command)
			=> new SelectedParameterInvocation<TIn, TOut, _>(command, _subject.Get).Then();

		public CommandContext<(TIn, T)> Terminate<T>(IAssign<TOut, T> assign) => Terminate<T>(assign.Assign);

		public CommandContext<(TIn, T)> Terminate<T>(Action<TOut, T> command)
			=> new SelectedAssignment<TIn, TOut, T>(_subject.Get, command).Then();

		public OperationResultSelector<TIn, TOut> Operation()
			=> new OperationResultSelector<TIn, TOut>(_subject.Select(x => x.ToOperation()));

		public ISelect<TIn, TOut> Get() => _subject;
	}
}