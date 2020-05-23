﻿using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public sealed class OperationContext<T> : Selector<T, ValueTask>
	{
		public static implicit operator Operate<T>(OperationContext<T> instance) => instance.Get().Get;

		public static implicit operator Await<T>(OperationContext<T> instance) => instance.Get().Await!; // ISSUE: NRT

		readonly ISelect<T, ValueTask> _subject;

		public OperationContext(ISelect<T, ValueTask> subject) : base(subject) => _subject = subject;

		public LogOperationContext<T, TParameter> Bind<TParameter>(ILogMessage<TParameter> log)
			=> new LogOperationContext<T, TParameter>(_subject, log);

		public LogOperationExceptionContext<T, TParameter> Bind<TParameter>(ILogException<TParameter> log)
			=> new LogOperationExceptionContext<T, TParameter>(_subject, log);

		public OperationContext<T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

		public OperationContext<T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

		public OperationContext<T> Watching(Func<CancellationToken> token)
			=> new OperationContext<T>(new TokenAwareOperation<T>(Get(), token));
	}

	sealed class TokenAwareOperation<T> : IOperation<T>
	{
		readonly ISelect<T, ValueTask> _operation;
		readonly Func<CancellationToken> _token;

		public TokenAwareOperation(ISelect<T, ValueTask> operation, Func<CancellationToken> token)
		{
			_operation = operation;
			_token     = token;
		}

		public async ValueTask Get(T parameter)
		{
			var token = _token();
			token.ThrowIfCancellationRequested();
			await _operation.Await(parameter);
			token.ThrowIfCancellationRequested();
		}
	}
}