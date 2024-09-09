using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class OutputOtherwiseThrowComposer<TIn, TOut>
{
	readonly Func<Func<TIn, string>, ICommand<TIn>> _guard;
	readonly OutputOtherwiseComposer<TIn, TOut>      _input;

	public OutputOtherwiseThrowComposer(OutputOtherwiseComposer<TIn, TOut> input,
	                                   Func<Func<TIn, string>, ICommand<TIn>> guard)
	{
		_input = input;
		_guard = guard;
	}

	public Composer<TIn, TOut> WithMessage(string message) => WithMessage(Start.A.Result(message));

	public Composer<TIn, TOut> WithMessage(IResult<string> message) => WithMessage(message.Get);

	public Composer<TIn, TOut> WithMessage(Func<string> message)
		=> WithMessage(message.Start().Accept<TIn>());

	public Composer<TIn, TOut> WithMessage(ISelect<TIn, string> message) => WithMessage(message.Get);

	public Composer<TIn, TOut> WithMessage(Func<TIn, string> message) => _input.Use(_guard(message));
}