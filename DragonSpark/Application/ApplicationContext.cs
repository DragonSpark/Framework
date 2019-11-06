using System;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application
{
	public class ApplicationContext<TIn, TOut> : Select<TIn, TOut>, IApplicationContext<TIn, TOut>
	{
		readonly IDisposable _disposable;

		public ApplicationContext(ISelect<TIn, TOut> select, IDisposable disposable) : base(select)
			=> _disposable = disposable;

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}

	public class ApplicationContext<T> : Command<T>, IApplicationContext<T>
	{
		readonly IDisposable _disposable;

		public ApplicationContext(ICommand<T> command, IDisposable disposable) : base(command)
			=> _disposable = disposable;

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}