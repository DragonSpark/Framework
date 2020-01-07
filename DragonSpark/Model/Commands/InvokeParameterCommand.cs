using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Model.Commands
{
	sealed class InvokeParameterCommand<T> : Command<T>
	{
		public InvokeParameterCommand(Func<T, None> @delegate) : base(new InvokeParameterCommand<T, None>(@delegate)) {}
	}

	sealed class InvokeParameterCommand<TIn, TOut> : ICommand<TIn>, IActivateUsing<Func<TIn, TOut>>
	{
		readonly Func<TIn, TOut> _delegate;

		public InvokeParameterCommand(Func<TIn, TOut> @delegate) => _delegate = @delegate;

		public void Execute(TIn parameter)
		{
			_delegate(parameter);
		}
	}
}