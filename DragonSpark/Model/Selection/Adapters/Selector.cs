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

		public Selector<_, T> OnceStriped() => new Selector<_, T>(OncePerParameter<_, T>.Default.Get(_subject));

		public Selector<_, T> OnlyOnce() => new Selector<_, T>(OnlyOnceAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Protect() => new Selector<_, T>(ProtectAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Stripe() => new Selector<_, T>(StripedAlteration<_, T>.Default.Get(_subject));

		public Selector<_, T> Try<TException>() where TException : Exception
			=> new Selector<_, T>(new Try<TException, _, T>(Get().Get));

		public CommandSelector<_> Terminate() => new CommandSelector<_>(new InvokeParameterCommand<_, T>(Get().Get));

		public CommandSelector<_> Terminate(ICommand<T> command) => Terminate(command.Execute);

		public CommandSelector<_> Terminate(System.Action<T> command)
			=> new CommandSelector<_>(new SelectedParameterCommand<_, T>(command, _subject.Get));
	}
}