using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;
using DragonSpark.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Selections
{
	public sealed class OperationContext<T> : Selector<T, ValueTask>
	{
		readonly ISelect<T, ValueTask> _subject;

		public OperationContext(ISelect<T, ValueTask> subject) : base(subject) => _subject = subject;

		public LogOperationContext<T, TParameter> Bind<TParameter>(ILogMessage<TParameter> log)
			=> new LogOperationContext<T, TParameter>(_subject, log);

		public LogOperationExceptionContext<T, TParameter> Bind<TParameter>(ILogException<TParameter> log)
			=> new LogOperationExceptionContext<T, TParameter>(_subject, log);
	}

	public sealed class LogOperationContext<T, TParameter>
	{
		readonly ISelect<T, ValueTask>   _operation;
		readonly ILogMessage<TParameter> _log;

		public LogOperationContext(ISelect<T, ValueTask> operation, ILogMessage<TParameter> log)
		{
			_operation = operation;
			_log       = log;
		}

		public IOperation<T> WithArguments(Func<(T Parameter, ValueTask Task), TParameter> @delegate)
			=> _operation.Then()
			             .Configure(@delegate.Start().Then().Terminate(_log).Get())
			             .Out();

		public IOperation<T> WithArguments(Func<T, TParameter> @delegate)
			=> WithArguments(new ParameterSelection<T, TParameter>(@delegate).Get);
	}

	public sealed class LogOperationExceptionContext<TIn, TOut>
	{
		readonly ISelect<TIn, ValueTask> _operation;
		readonly ILogException<TOut> _log;

		public LogOperationExceptionContext(ISelect<TIn, ValueTask> operation, ILogException<TOut> log)
		{
			_operation = operation;
			_log       = log;
		}

		public IOperation<TIn> WithArguments(Parameter<TIn, TOut> @delegate)
			=> new LogOperationExceptionBinding<TIn, TOut>(_operation, @delegate, _log);

		public IOperation<TIn> WithArguments(Func<TIn, TOut> @delegate)
			=> WithArguments(new ParameterSelection<TIn, TOut>(@delegate).Get);
	}

	public delegate TOut Parameter<TIn, out TOut>((TIn Parameter, ValueTask Task) context);

	sealed class ParameterAdapter<TIn, TOut> : ISelect<(TIn Parameter, ValueTask Task), TOut>
	{
		readonly Parameter<TIn, TOut> _parameter;

		public ParameterAdapter(Parameter<TIn, TOut> parameter) => _parameter = parameter;

		public TOut Get((TIn Parameter, ValueTask Task) parameter) => _parameter(parameter);
	}

	sealed class ParameterSelection<TIn, TOut> : ISelect<(TIn Parameter, ValueTask Task), TOut>
	{
		readonly Func<TIn, TOut> _select;

		public ParameterSelection(Func<TIn, TOut> select) => _select = select;

		public TOut Get((TIn Parameter, ValueTask Task) parameter) => _select(parameter.Parameter);
	}

	sealed class LogOperationExceptionBinding<TIn, TOut> : IOperation<TIn>
	{
		readonly ISelect<TIn, ValueTask> _operation;
		readonly Parameter<TIn, TOut>  _select;
		readonly ILogException<TOut> _log;

		public LogOperationExceptionBinding(ISelect<TIn, ValueTask> operation, Parameter<TIn, TOut> select, ILogException<TOut> log)
		{
			_operation = operation;
			_select    = select;
			_log       = log;
		}

		public ValueTask Get(TIn parameter)
		{
			var result = _operation.Get(parameter);

			if (result.IsFaulted)
			{
				var input = new ExceptionParameter<TOut>(result.AsTask().Exception, _select((parameter, result)));
				_log.Execute(input);
			}

			return result;
		}
	}
}
