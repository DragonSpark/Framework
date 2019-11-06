using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Selection.Adapters
{
	public class CommandSelector : CommandSelector<None>
	{
		public CommandSelector(ICommand command) : base(command) => Command = command;

		public ICommand Command { get; }

		public static implicit operator System.Action(CommandSelector instance) => instance.Get().ToDelegate();

		public CommandSelector<object> Any() => new CommandSelector<object>(new Any(Get()));
	}

	public class CommandSelector<T> : Instance<ICommand<T>>
	{
		public static implicit operator System.Action<T>(CommandSelector<T> instance) => instance.Get().ToDelegate();

		public CommandSelector(ICommand<T> command) : base(command) {}

		public CommandSelector Input(T parameter = default)
			=> new CommandSelector(new FixedParameterCommand<T>(Get().Execute, parameter));

		public CommandSelector Input(IResult<T> parameter)
			=> new CommandSelector(new DelegatedParameterCommand<T>(Get().Execute, parameter.Get));

		public CommandSelector<T> Then(params ICommand<T>[] commands)
			=> new CommandSelector<T>(new CompositeCommand<T>(commands.Prepend(Get()).Result()));

		public CommandSelector<Sequences.Store<T>> Many()
			=> new CommandSelector<Sequences.Store<T>>(new ManyCommand<T>(Get()));

		/*public Selector<T, None> ToSelector() => new Selector<T, None>(Get().ToSelect());*/

		public AlterationSelector<T> ToConfiguration() => new AlterationSelector<T>(new Configured<T>(Get().Execute));
	}
}