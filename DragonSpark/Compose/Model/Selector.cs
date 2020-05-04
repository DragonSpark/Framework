using DragonSpark.Compose.Model.Validation;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
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
		public static implicit operator Func<TIn, TOut>(Selector<TIn, TOut> instance) => instance.Get().Get;

		public static implicit operator NetFabric.Hyperlinq.Selector<TIn, TOut>(Selector<TIn, TOut> instance) => instance.Get().Get;

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

		public Selector<TIn, TOut> Configure(IAssign<TIn, TOut> configuration)
			=> new Configuration<TIn, TOut>(_subject, configuration).Then();

		public Selector<TIn, TOut> Configure(ICommand<(TIn, TOut)> configuration)
			=> new Configuration<TIn, TOut>(_subject.Get, configuration.Execute!).Then(); // TODO: https://youtrack.jetbrains.com/issue/RSRP-479406

		public Selector<TIn, TOut> Configure<TOther>(IAssign<TIn, TOther> configuration)
			=> new Configuration<TIn, TOut, TOther>(_subject, configuration).Then();

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

#pragma warning disable 8714
		public Selector<TIn, TOut> OnceStriped() => OncePerParameter<TIn, TOut>.Default.Get(_subject).Then();
#pragma warning restore 8714

		public Selector<TIn, TOut> OnlyOnce() => OnlyOnceAlteration<TIn, TOut>.Default.Get(_subject).Then();

		public Selector<TIn, TOut> Protect() => ProtectAlteration<TIn, TOut>.Default.Get(_subject).Then();

#pragma warning disable 8714
		public Selector<TIn, TOut> Stripe() => StripedAlteration<TIn, TOut>.Default.Get(_subject).Then();
#pragma warning restore 8714

		public Selector<TIn, TOut> Try<TException>() where TException : Exception
			=> new Try<TException, TIn, TOut>(Get().Get).Then();

		public CommandContext<TIn> Terminate() => new InvokeParameterCommand<TIn, TOut>(Get().Get).Then();

		public CommandContext<TIn> Terminate(ICommand<TOut> command) => Terminate(command.Execute);

		public CommandContext<TIn> Terminate(System.Action<TOut> command)
			=> new SelectedParameterCommand<TIn, TOut>(command, _subject.Get).Then();

		public ISelect<TIn, TOut> Get() => _subject;
	}
}