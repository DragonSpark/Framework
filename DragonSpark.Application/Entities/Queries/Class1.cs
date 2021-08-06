using DragonSpark.Application.Entities.Selection;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public class Where<TKey, T> : IQuery<TKey, T>
	{
		readonly IQueryable<T>  _source;
		readonly Query<TKey, T> _query;

		protected Where(IQueryable<T> source, Query<TKey, T> query)
		{
			_source = source;
			_query  = query;
		}

		public IQueryable<T> Get(TKey parameter) => _source.Where(_query(parameter));
	}

	public class WhereSelect<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>          _source;
		readonly Query<TKey, TEntity>         _query;
		readonly Expression<Func<TEntity, T>> _select;

		protected WhereSelect(IQueryable<TEntity> source, Query<TKey, TEntity> query,
		                      Expression<Func<TEntity, T>> select)
		{
			_source = source;
			_query  = query;
			_select = select;
		}

		public IQueryable<T> Get(TKey parameter) => _source.Where(_query(parameter)).Select(_select);
	}

	public class ParameterAwareWhereSelect<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>     _queryable;
		readonly Query<TKey, TEntity>    _query;
		readonly Query<TKey, TEntity, T> _selection;

		protected ParameterAwareWhereSelect(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                    Query<TKey, TEntity, T> selection)
		{
			_queryable = queryable;
			_query     = query;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter) => _queryable.Where(_query(parameter)).Select(_selection(parameter));
	}

	public class ParameterAwareWhereSelection<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>                            _queryable;
		readonly Query<TKey, TEntity>                           _query;
		readonly Func<TKey, IQueryable<TEntity>, IQueryable<T>> _selection;

		protected ParameterAwareWhereSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                       Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_queryable = queryable;
			_query     = query;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter)
		{
			var query  = _queryable.Where(_query(parameter));
			var result = _selection(parameter, query);
			return result;
		}
	}

	public class WhereMany<TKey, T> : WhereMany<TKey, T, T>
	{
		public WhereMany(IQueryable<T> queryable, Query<TKey, T> query, Expression<Func<T, IEnumerable<T>>> select)
			: base(queryable, query, select) {}
	}

	public class WhereMany<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>                       _queryable;
		readonly Query<TKey, TEntity>                      _query;
		readonly Expression<Func<TEntity, IEnumerable<T>>> _select;

		protected WhereMany(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                    Expression<Func<TEntity, IEnumerable<T>>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public IQueryable<T> Get(TKey parameter) => _queryable.Where(_query(parameter)).SelectMany(_select);
	}

	public interface IQuery<in TKey, out T> : ISelect<TKey, IQueryable<T>> {}

	public class Materialize<TIn, TEntity, TResult> : ISelecting<TIn, TResult>
	{
		readonly IQuery<TIn, TEntity>            _query;
		readonly IMaterializer<TEntity, TResult> _materializer;

		protected Materialize(IQuery<TIn, TEntity> query, IMaterializer<TEntity, TResult> materializer)
		{
			_query        = query;
			_materializer = materializer;
		}

		public ValueTask<TResult> Get(TIn parameter) => _materializer.Get(_query.Get(parameter));
	}

	public class ToArray<TIn, TEntity> : Materialize<TIn, TEntity, Array<TEntity>>
	{
		protected ToArray(IQuery<TIn, TEntity> query) : this(query, DefaultToArray<TEntity>.Default) {}

		protected ToArray(IQuery<TIn, TEntity> query, IMaterializer<TEntity, Array<TEntity>> materializer)
			: base(query, materializer) {}
	}

	public class ToList<TIn, TEntity> : Materialize<TIn, TEntity, List<TEntity>>
	{
		protected ToList(IQuery<TIn, TEntity> query) : this(query, DefaultToList<TEntity>.Default) {}

		protected ToList(IQuery<TIn, TEntity> query, IMaterializer<TEntity, List<TEntity>> materializer)
			: base(query, materializer) {}
	}

	public class ToDictionary<TIn, TKey, T> : Materialize<TIn, T, IReadOnlyDictionary<TKey, T>>
	{
		protected ToDictionary(IQuery<TIn, T> query, Func<T, TKey> key)
			: this(query, new DictionaryMaterializer<T, TKey>(key)) {}

		protected ToDictionary(IQuery<TIn, T> query, IMaterializer<T, IReadOnlyDictionary<TKey, T>> materializer)
			: base(query, materializer) {}
	}

	public class SingleOrDefault<TIn, T> : Materialize<TIn, T, T?>
	{
		protected SingleOrDefault(IQuery<TIn, T> query) : this(query, SingleOrDefaultMaterializer<T>.Default) {}

		protected SingleOrDefault(IQuery<TIn, T> query, IMaterializer<T, T?> materializer) :
			base(query, materializer) {}
	}

	public class Single<TIn, T> : Materialize<TIn, T, T>
	{
		protected Single(IQuery<TIn, T> query) : this(query, SingleMaterializer<T>.Default) {}

		protected Single(IQuery<TIn, T> query, IMaterializer<T, T> materializer) : base(query, materializer) {}
	}

	public sealed class SingleOrDefaultMaterializer<T> : IMaterializer<T, T?>
	{
		public static SingleOrDefaultMaterializer<T> Default { get; } = new SingleOrDefaultMaterializer<T>();

		SingleOrDefaultMaterializer() {}

		public async ValueTask<T?> Get(IQueryable<T> parameter)
		{
			var entity = await parameter.SingleOrDefaultAsync().ConfigureAwait(false);
			var result = entity.Account();
			return result;
		}
	}

	public sealed class SingleMaterializer<T> : IMaterializer<T, T>
	{
		public static SingleMaterializer<T> Default { get; } = new();

		SingleMaterializer() {}

		public ValueTask<T> Get(IQueryable<T> parameter) => parameter.SingleAsync().ToOperation();
	}

	public class DictionaryMaterializer<T, TKey> : DictionaryMaterializer<T, TKey, T>
	{
		public DictionaryMaterializer(Func<T, TKey> key) : base(key, x => x) {}
	}

	public class DictionaryMaterializer<T, TKey, TValue> : IMaterializer<T, IReadOnlyDictionary<TKey, TValue>>
	{
		readonly Func<T, TKey>   _key;
		readonly Func<T, TValue> _value;

		public DictionaryMaterializer(Func<T, TKey> key, Func<T, TValue> value)
		{
			_key   = key;
			_value = value;
		}

		public async ValueTask<IReadOnlyDictionary<TKey, TValue>> Get(IQueryable<T> parameter)
			=> await parameter.ToDictionaryAsync(_key, _value).ConfigureAwait(false);
	}
}