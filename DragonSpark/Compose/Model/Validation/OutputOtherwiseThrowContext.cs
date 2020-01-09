using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation {
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

		public Selector<TIn, TOut> WithMessage(IResult<string> message)
			=> WithMessage(message.Then().Accept<TIn>().Return());

		public Selector<TIn, TOut> WithMessage(ISelect<TIn, string> message) => _input.Use(_guard(message));
	}
}