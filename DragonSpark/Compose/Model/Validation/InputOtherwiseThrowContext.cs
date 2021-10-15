using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class InputOtherwiseThrowContext<TIn, TOut>
{
	readonly Func<Func<TIn, string>, ICommand<TIn>> _guard;
	readonly InputOtherwiseContext<TIn, TOut>       _input;

	public InputOtherwiseThrowContext(InputOtherwiseContext<TIn, TOut> input,
	                                  Func<Func<TIn, string>, ICommand<TIn>> guard)
	{
		_input = input;
		_guard = guard;
	}

	public ConditionalSelector<TIn, TOut> WithMessage(string message) => WithMessage(Start.A.Result(message));

	public ConditionalSelector<TIn, TOut> WithMessage(IResult<string> message) => WithMessage(message.Get);

	public ConditionalSelector<TIn, TOut> WithMessage(Func<string> message)
		=> WithMessage(message.Start().Accept<TIn>().Return());

	public ConditionalSelector<TIn, TOut> WithMessage(ISelect<TIn, string> message) => WithMessage(message.Get);

	public ConditionalSelector<TIn, TOut> WithMessage(Func<TIn, string> message) => _input.Use(_guard(message));
}