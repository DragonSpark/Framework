using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape
{
	class Class1 {}

	public class QueryInput
	{
		public bool IncludeTotalCount { get; set; }

		public string? OrderBy { get; set; }

		public string? Filter { get; set; }

		public Partition? Partition { get; set; }

		/*public IEnumerable<FilterDescriptor> Filters { get; set; }

		public IEnumerable<SortDescriptor> Sorts { get; set; }*/
	}

	public readonly record struct Partition(int? Skip, int? Top);

	public readonly record struct PagingInput<T>(IQueries<T> Queries, ICompose<T> Compose);

	public interface IPagers<T> : ISelect<PagingInput<T>, IPaging<T>> {}

	public sealed class Pagers<T> : IPagers<T>
	{
		public static Pagers<T> Default { get; } = new Pagers<T>();

		Pagers() {}

		public IPaging<T> Get(PagingInput<T> parameter) => new Paging<T>(parameter.Queries, parameter.Compose);
	}

	/*
	public class ExceptionAwareEvaluations<T> : IEvaluations<T>
	{
		readonly IEvaluations<T> _previous;
		readonly IExceptions     _logger;
		readonly Type            _reportedType;

		public ExceptionAwareEvaluations(IEvaluations<T> previous, IExceptions logger, Type reportedType)
		{
			_previous     = previous;
			_logger       = logger;
			_reportedType = reportedType;
		}

		public IEvaluate<T> Get(EvaluationInput<T> parameter)
			=> new ExceptionAwareEvaluate<T>(_previous.Get(parameter), _logger, _reportedType);
	}
	*/

	/*
	sealed class ExceptionAwareEvaluate<T> : IEvaluate<T>
	{
		readonly IEvaluate<T> _previous;
		readonly IExceptions  _logger;
		readonly Type         _reportedType;

		public ExceptionAwareEvaluate(IEvaluate<T> previous, IExceptions logger, Type reportedType)
		{
			_previous     = previous;
			_logger       = logger;
			_reportedType = reportedType;
		}

		public async ValueTask<Current<T>> Get(QueryInput parameter)
		{
			try
			{
				return await _previous.Await(parameter);
			}
			catch (Exception e)
			{
				await _logger.Await(_reportedType, e);
				throw;
			}
		}
	}
	*/

	public sealed class EmptyPaging<T> : IPaging<T>
	{
		public static EmptyPaging<T> Default { get; } = new();

		EmptyPaging() {}

		public ValueTask<Current<T>> Get(QueryInput parameter) => Current<T>.Default.ToOperation();
	}

	public interface IPaging<T> : ISelecting<QueryInput, Current<T>> {}

	public sealed class Paging<T> : IPaging<T>
	{
		readonly IQueries<T> _queries;
		readonly ICompose<T> _compose;
		readonly IToArray<T> _materialize;

		public Paging(IQueries<T> queries, ICompose<T> compose) : this(queries, compose, DefaultToArray<T>.Default) {}

		public Paging(IQueries<T> queries, ICompose<T> compose, IToArray<T> materialize)
		{
			_queries     = queries;
			_compose     = compose;
			_materialize = materialize;
		}

		public async ValueTask<Current<T>> Get(QueryInput parameter)
		{
			using var session = await _queries.Await();
			var (query, count) = await _compose.Await(new(parameter, session.Subject));
			var materialize = await _materialize.Await(query);
			return new(materialize.Open(), count);
		}
	}

	public sealed class Current<T> : Collection<T>
	{
		public static Current<T> Default { get; } = new Current<T>();

		Current() : this(Empty.Array<T>(), null) {}

		public Current(IList<T> list, ulong? total) : base(list) => Total = total;

		public ulong? Total { get; }
	}

	public interface ICompose<T> : ISelecting<ComposeInput<T>, Composition<T>> {}

	public interface IBody<T> : ISelecting<ComposeInput<T>, IQueryable<T>> {}

	public readonly record struct ComposeInput<T>(QueryInput Input, IQueryable<T> Current);

	public readonly record struct Composition<T>(IQueryable<T> Current, ulong? Count = null);

	public sealed class DefaultCompose<T> : Compose<T>
	{
		public static DefaultCompose<T> Default { get; } = new();

		DefaultCompose() : base(Body<T>.Default) {}
	}

	public class Compose<T> : ICompose<T>
	{
		readonly IBody<T>       _body;
		readonly ILargeCount<T> _count;
		readonly IPartition<T>  _partition;

		public Compose(IBody<T> body) : this(body, DefaultLargeCount<T>.Default, Partitioning<T>.Default) {}

		public Compose(IBody<T> body, ILargeCount<T> count, IPartition<T> partition)
		{
			_body      = body;
			_count     = count;
			_partition = partition;
			_partition = partition;
		}

		public async ValueTask<Composition<T>> Get(ComposeInput<T> parameter)
		{
			var (input, _) = parameter;
			var body      = await _body.Await(parameter);
			var count     = input.IncludeTotalCount ? await _count.Await(body) : default(ulong?);
			var partition = input.Partition.HasValue ? await _partition.Await(new(body, input.Partition.Value)) : body;
			return new(partition, count);
		}
	}

	public sealed class FilteredBody<T> : AppendedBody<T>
	{
		public FilteredBody(string filter) : base(new Filter<T>(filter), Sort<T>.Default) {}
	}

	sealed class Body<T> : AppendedBody<T>
	{
		public static Body<T> Default { get; } = new Body<T>();

		Body() : base(Where<T>.Default, Sort<T>.Default) {}
	}

	public sealed class Where<T> : IBody<T>
	{
		public static Where<T> Default { get; } = new Where<T>();

		Where() {}

		public ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
		{
			var (input, current) = parameter;
			var queryable = !string.IsNullOrEmpty(input.Filter) ? current.Where(input.Filter) : current;
			var result    = queryable.ToOperation();
			return result;
		}
	}

	public sealed class Filter<T> : IBody<T>
	{
		readonly string _filter;

		public Filter(string filter) => _filter = filter;

		public ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
		{
			var (input, current) = parameter;
			var queryable = input.Filter != null ? current.Where(_filter, input.Filter) : current;
			var result    = queryable.ToOperation();
			return result;
		}
	}

	public sealed class Sort<T> : IBody<T>
	{
		public static Sort<T> Default { get; } = new Sort<T>();

		Sort() {}

		public ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
		{
			var (input, current) = parameter;
			var queryable = !string.IsNullOrEmpty(input.OrderBy) ? current.OrderBy(input.OrderBy) : current;
			var result    = queryable.ToOperation();
			return result;
		}
	}

	public class AppendedBody<T> : IBody<T>
	{
		readonly IBody<T> _first;
		readonly IBody<T> _second;

		public AppendedBody(IBody<T> first, IBody<T> second)
		{
			_first  = first;
			_second = second;
		}

		public async ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
		{
			var first  = await _first.Await(parameter);
			var result = await _second.Await(new(parameter.Input, first));
			return result;
		}
	}

	public readonly record struct Partition<T>(IQueryable<T> Subject, Partition Input);

	public interface IPartition<T> : ISelecting<Partition<T>, IQueryable<T>> {}

	sealed class Partitioning<T> : IPartition<T>
	{
		public static Partitioning<T> Default { get; } = new Partitioning<T>();

		Partitioning() {}

		public ValueTask<IQueryable<T>> Get(Partition<T> parameter)
		{
			var (queryable, (skip, top)) = parameter;
			var first  = skip.HasValue ? queryable.Skip(skip.Value) : queryable;
			var result = top.HasValue ? first.Take(top.Value) : first;
			return result.ToOperation();
		}
	}
}