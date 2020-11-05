using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Collections;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
#pragma warning disable EF1001
	sealed class QueryProvider : IAsyncQueryProvider
	{
		readonly static ICondition<Type> IsTask = Is.AssignableFrom<Task>().Out();

		readonly IAsyncQueryProvider _previous;
		readonly AsyncLock           _lock;
		readonly IGeneric            _execute;

		public QueryProvider(IAsyncQueryProvider previous, AsyncLock @lock) : this(previous, @lock, Generic.Default) {}

		public QueryProvider(IAsyncQueryProvider previous, AsyncLock @lock, IGeneric execute)
		{
			_previous = previous;
			_lock     = @lock;
			_execute  = execute;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			var query  = _previous.CreateQuery(expression);
			var result = Caller.Default.Get(query.ElementType)().Get((this, query));
			return result;
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
			=> New(_previous.CreateQuery<TElement>(expression));

		ProtectedQuery<T> New<T>(IQueryable<T> previous)
			=> new ProtectedQuery<T>(new Querying<T>(previous), this, _lock);

		public object Execute(Expression expression) => _previous.Execute(expression);

		public TResult Execute<TResult>(Expression expression) => _previous.Execute<TResult>(expression);

		public TResult ExecuteAsync<TResult>(Expression expression,
		                                     CancellationToken cancellationToken = new CancellationToken())
		{
			if (IsTask.Get(A.Type<TResult>()))
			{
				var type = InnerType.Default.Get(A.Type<TResult>());
				var result = type != null
					             ? _execute.Get(type)(_previous, expression, cancellationToken, _lock)
					                       .Get()
					                       .To<TResult>()
					             : ExecuteTask.Default.Get((_previous, expression, cancellationToken, _lock))
					                          .To<TResult>();
				return result;
			}

			{
				var result = _previous.ExecuteAsync<TResult>(expression, cancellationToken);
				return result;
			}
		}

		internal interface IGeneric
			: IGeneric<IAsyncQueryProvider, Expression, CancellationToken, AsyncLock, IResult<object>> {}

		sealed class Generic : Generic<IAsyncQueryProvider, Expression, CancellationToken, AsyncLock, IResult<object>>,
		                       IGeneric
		{
			public static Generic Default { get; } = new Generic();

			Generic() : base(typeof(ExecuteTaskResult<>)) {}
		}

		sealed class ExecuteTaskResult<T> : IResult<Task<T>>, IResult<object>
		{
			readonly IAsyncQueryProvider _provider;
			readonly Expression          _expression;
			readonly CancellationToken   _cancellationToken;
			readonly AsyncLock           _lock;

			// ReSharper disable once TooManyDependencies
			public ExecuteTaskResult(IAsyncQueryProvider provider, Expression expression,
			                         CancellationToken cancellationToken, AsyncLock @lock)
			{
				_provider          = provider;
				_expression        = expression;
				_cancellationToken = cancellationToken;
				_lock              = @lock;
			}

			public async Task<T> Get()
			{
				using (await _lock.LockAsync(_cancellationToken))
				{
					return await _provider.ExecuteAsync<Task<T>>(_expression, _cancellationToken);
				}
			}

			object IResult<object>.Get() => Get();
		}

		sealed class ExecuteTask
			: ISelect<(IAsyncQueryProvider provider, Expression expression,
				CancellationToken cancellationToken, AsyncLock @lock), Task>
		{
			public static ExecuteTask Default { get; } = new ExecuteTask();

			ExecuteTask() {}

			public async Task Get((IAsyncQueryProvider provider, Expression expression,
				                      CancellationToken cancellationToken, AsyncLock @lock) parameter)
			{
				var (provider, expression, token, @lock) = parameter;
				using (await @lock.LockAsync(token))
				{
					await provider.ExecuteAsync<Task>(expression, token);
				}
			}
		}

		sealed class Caller : Generic<ISelect<(QueryProvider, IQueryable), IQueryable>>
		{
			public static Caller Default { get; } = new Caller();

			Caller() : base(typeof(Caller<>)) {}
		}

		sealed class Caller<T> : ISelect<(QueryProvider, IQueryable), IQueryable>
		{
			[UsedImplicitly]
			public static Caller<T> Default { get; } = new Caller<T>();

			Caller() {}

			public IQueryable Get((QueryProvider, IQueryable) parameter)
				=> parameter.Item2 is IQueryable<T> queryable
					   ? parameter.Item1.New(queryable)
					   : parameter.Item2;
		}
	}
#pragma warning restore
}