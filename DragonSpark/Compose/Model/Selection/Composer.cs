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

namespace DragonSpark.Compose.Model.Selection;

public class Composer<T> : Composer<None, T>
{
	public static implicit operator Func<T>(Composer<T> instance) => instance.Get().Get;

	public Composer(ISelect<None, T> subject) : base(subject) {}
}

public class Composer<TIn, TOut> : IResult<ISelect<TIn, TOut>>, IActivateUsing<ISelect<TIn, TOut>>
{
	public static implicit operator Func<TIn, TOut>(Composer<TIn, TOut> instance) => instance._subject.Get;

	readonly ISelect<TIn, TOut> _subject;

	public Composer(ISelect<TIn, TOut> subject) => _subject = subject;

	public EnsureSelectionComposer<TIn, TOut> Ensure => new(_subject);

	public UnlessComposer<TIn, TOut> Unless => new(_subject);

	public TypeComposer<TIn> Type() => new(_subject.Select(InstanceType<TOut>.Default));

	public Composer<T, TOut> Accept<T>(Func<T, TIn> select)
		=> Start.A.Selection<T>().By.Calling(select).Select(_subject);

	public Composer<TIn, TTo> StoredActivation<TTo>() where TTo : IActivateUsing<TOut>
		=> Select(Activations<TOut, TTo>.Default);

	public ResultComposer<TOut> Bind(TIn parameter) => new FixedSelection<TIn, TOut>(_subject, parameter).Then();

	public ResultComposer<TOut> Bind(IResult<TIn> parameter) => Bind(parameter.Get);

	public ResultComposer<TOut> Bind(Func<TIn> parameter)
		=> new SelectedResult<TIn, TOut>(parameter, _subject.Get).Then();

	public Composer<TIn, TTo> Select<TTo>(Composer<TOut, TTo> select) => Select(select.Get());

	public Composer<TIn, TTo> Select<TTo>(ISelect<TOut, TTo> select) => Select(select.Get);

	public Composer<TIn, TTo> Select<TTo>(Func<TOut, TTo> select)
		=> new Selection<TIn, TOut, TTo>(_subject.Get, select).Then();

	public Composer<TIn, TOut> Configure(IAssign<TIn, TOut> configure) => Configure(configure.Assign);

	public Composer<TIn, TOut> Configure(ICommand<(TIn, TOut)> configuration) => Configure(configuration.Execute);

	public Composer<TIn, TOut> Configure(Action<TIn, TOut> configure)
		=> new Configure<TIn, TOut>(_subject.Get, configure).Then();

	public Composer<TIn, TOut> Configure(ICommand<TOut> configure) => Configure(configure.Execute);

	public Composer<TIn, TOut> Configure(System.Action<TOut> configure)
		=> new ConfigureOutput<TIn, TOut>(_subject.Get, configure).Then();

	public SequenceComposer<TIn, TOut> Yield() => new(Select(x => x.Yield()).Get());

	public Composer<TIn?, TOut> EnsureAssignedOrDefault() => OrDefault(Is.Assigned<TIn>())!;

	public Composer<TIn, TOut> OrDefault(ISelect<TIn, bool> use) => OrDefault(use.Get);

	public Composer<TIn, TOut> OrDefault(Func<TIn, bool> use)
		=> OrDefault(use, Start.A.Selection<TIn>().By.Default<TOut>());

	public Composer<TIn, TOut> OrDefault(Func<TIn, bool> use, Func<TIn, TOut> @default)
		=> Ensure.Input.Is(use).Otherwise.Use(@default);

	public Composer<TIn, TTo> Default<TTo>() => Select(Default<TOut, TTo>.Instance);

	public Composer<TIn, TTo> Cast<TTo>() => Select(CastOrDefault<TOut, TTo>.Default);

	public Composer<TIn, TTo> CastForResult<TTo>() => Select(ResultAwareCast<TOut, TTo>.Default);

	public Composer<TIn, (TIn, TOut)> Introduce() => new Introduce<TIn, TOut>(_subject).Then();

	public Composer<TIn, TOut> OnlyOnce() => OnlyOnceAlteration<TIn, TOut>.Default.Get(_subject).Then();

	public Composer<TIn, TOut> Protect() => ProtectAlteration<TIn, TOut>.Default.Get(_subject).Then();

#pragma warning disable 8714
	public Composer<TIn, TOut> Stripe() => StripedAlteration<TIn, TOut>.Default.Get(_subject).Then();

	public Composer<TIn, TOut> OnceStriped() => OncePerParameter<TIn, TOut>.Default.Get(_subject).Then();

	public TableComposer<TIn, T> Table<T>() => Table(Tables<TOut, T>.Default.Get(_ => default!));

	public TableComposer<TIn, T> Table<T>(ITable<TOut, T> table)
		=> new DelegatedTable<TIn, T>(Select(table.Condition.Get), Terminate(table), Select(table.Get),
		                              Select(table.Remove)).Then();
#pragma warning restore 8714

	public Composer<TIn, TOut> Try<TException>() where TException : Exception
		=> new Try<TException, TIn, TOut>(_subject.Get).Then();

	public CommandComposer<TIn> Terminate() => new InvokeParameterCommand<TIn, TOut>(this).Then();

	public CommandComposer<TIn> Terminate(ICommand<TOut> command) => Terminate(command.Execute);

	public CommandComposer<TIn> Terminate(System.Action<TOut> command)
		=> new SelectedParameterCommand<TIn, TOut>(command, _subject.Get).Then();

	public CommandComposer<TIn> Terminate<_>(Func<TOut, _> command)
		=> new SelectedParameterInvocation<TIn, TOut, _>(command, _subject.Get).Then();

	public CommandComposer<(TIn, T)> Terminate<T>(IAssign<TOut, T> assign) => Terminate<T>(assign.Assign);

	public CommandComposer<(TIn, T)> Terminate<T>(Action<TOut, T> command)
		=> new SelectedAssignment<TIn, TOut, T>(_subject.Get, command).Then();

	public OperationResultComposer<TIn, TOut> Operation() => new(_subject.Select(x => x.ToOperation()));
	/*public Composer<TIn, Stop<TOut>> Stop() => new(_subject.Select(x => x.Stop()));*/

	public ISelect<TIn, TOut> Get() => _subject;
}