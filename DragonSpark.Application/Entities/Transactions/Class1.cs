using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IScopedTransactions>()
			         .Forward<ScopedTransactions>()
			         .Singleton()
			         //
			         .Then.Start<ITransactions>()
			         .Forward<Transactions>()
			         .Singleton();
		}
	}

	public interface IScopedTransaction : ITransaction
	{
		IServiceProvider Provider { get; }
	}

	sealed class ScopedTransaction : IScopedTransaction
	{
		readonly ICommand<AsyncServiceScope?> _store;
		readonly AsyncServiceScope            _instance;

		public ScopedTransaction(ICommand<AsyncServiceScope?> store, AsyncServiceScope instance)
			: this(store, instance, instance.ServiceProvider) {}

		public ScopedTransaction(ICommand<AsyncServiceScope?> store, AsyncServiceScope instance,
		                         IServiceProvider provider)
		{
			Provider  = provider;
			_store    = store;
			_instance = instance;
		}

		public IServiceProvider Provider { get; }

		public ValueTask DisposeAsync()
		{
			_store.Execute(default);
			return _instance.DisposeAsync();
		}

		public ValueTask Get()
		{
			return ValueTask.CompletedTask;
		}

		public void Execute(None parameter)
		{
			_store.Execute(_instance);
		}
	}

	public interface ITransaction : ICommand, IOperation, IAsyncDisposable {}

	public interface ITransactions : IResulting<ITransaction> {}

	sealed class Transactions : ITransactions
	{
		readonly IScopedTransactions _transactions;

		public Transactions(IScopedTransactions transactions) => _transactions = transactions;

		public ValueTask<ITransaction> Get() => _transactions.Get().ToOperation<ITransaction>();
	}

	public sealed class DatabaseTransactions : ITransactions
	{
		readonly IScopedTransactions _boundaries;

		public DatabaseTransactions(IScopedTransactions boundaries) => _boundaries = boundaries;

		public async ValueTask<ITransaction> Get()
		{
			var previous = _boundaries.Get();
			var transaction = await previous.Provider.GetRequiredService<DbContext>()
			                                .Database.BeginTransactionAsync()
			                                .ConfigureAwait(false);
			var result = new DatabaseTransaction(previous, transaction);
			return result;
		}
	}

	sealed class DatabaseTransaction : ITransaction
	{
		readonly ITransaction          _previous;
		readonly IDbContextTransaction _transaction;

		public DatabaseTransaction(ITransaction previous, IDbContextTransaction transaction)
		{
			_previous    = previous;
			_transaction = transaction;
		}

		public async ValueTask Get()
		{
			await _previous.Await();
			await _transaction.CommitAsync().ConfigureAwait(false);
		}

		public async ValueTask DisposeAsync()
		{
			await _previous.DisposeAsync().ConfigureAwait(false);
			await _transaction.DisposeAsync().ConfigureAwait(false);
		}

		public void Execute(None parameter)
		{
			_previous.Execute(parameter);
		}
	}

	public interface IScopedTransactions : IResult<IScopedTransaction> {}

	public sealed class ScopedTransactions : IScopedTransactions
	{
		readonly IScopes                      _scopes;
		readonly IMutable<AsyncServiceScope?> _store;

		public ScopedTransactions(IScopes scopes) : this(scopes, LogicalScope.Default) {}

		public ScopedTransactions(IScopes scopes, IMutable<AsyncServiceScope?> store)
		{
			_scopes = scopes;
			_store  = store;
		}

		public IScopedTransaction Get() => new ScopedTransaction(_store, _scopes.Get());
	}

	sealed class AmbientAwareContexts<T> : IContexts<T> where T : DbContext
	{
		readonly IContexts<T>               _previous;
		readonly IResult<IServiceProvider?> _store;

		public AmbientAwareContexts(IContexts<T> previous) : this(previous, AmbientProvider.Default) {}

		public AmbientAwareContexts(IContexts<T> previous, IResult<IServiceProvider?> store)
		{
			_previous = previous;
			_store    = store;
		}

		public T Get()
		{
			var current = _store.Get();
			var result  = current != null ? current.GetRequiredService<T>() : _previous.Get();
			return result;
		}
	}
}