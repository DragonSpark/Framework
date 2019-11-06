using System;
using DragonSpark.Compose.Conditions;

namespace DragonSpark.Compose
{
	public sealed class ModelContext
	{
		public static ModelContext Default { get; } = new ModelContext();

		ModelContext() : this(() => Context.Default, () => Results.Context.Default,
		                      () => Commands.Context.Default, () => Selections.Context.Default) {}

		readonly Func<Commands.Context> _command;

		readonly Func<Context>            _condition;
		readonly Func<Results.Context>    _result;
		readonly Func<Selections.Context> _selection;

		// ReSharper disable once TooManyDependencies
		public ModelContext(Func<Context> condition, Func<Results.Context> result, Func<Commands.Context> command,
		                    Func<Selections.Context> selection)
		{
			_condition = condition;
			_result    = result;
			_command   = command;
			_selection = selection;
		}

		public Context Condition => _condition();

		public Results.Context Result => _result();

		public Commands.Context Command => _command();

		public Selections.Context Selection => _selection();

		public Generics.Context Generic(Type definition) => new Generics.Context(definition);
	}
}