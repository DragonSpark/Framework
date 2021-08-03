using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	public sealed class TableStoreContext<TIn, TOut>
	{
		readonly ISelect<TIn, ValueTask<TOut>> _subject;
		readonly ITable<string, object>        _storage;

		public TableStoreContext(ISelect<TIn, ValueTask<TOut>> subject, ITable<string, object> storage)
		{
			_subject = subject;
			_storage = storage;
		}

		public OperationResultSelector<TIn, TOut> Using<T>(Func<TIn, string> key)
			=> Using(new Key<TIn>(A.Type<T>().AssemblyQualifiedName.Verify(), key).Get);

		public OperationResultSelector<TIn, TOut> Using(ISelect<TIn, string> key) => Using(key.Get);

		public OperationResultSelector<TIn, TOut> Using(Func<TIn, string> key)
			=> new Source(_storage, _subject.Get, key).Then();

		sealed class Source : ISelecting<TIn, TOut>
		{
			readonly ITable<string, object>     _store;
			readonly Func<TIn, ValueTask<TOut>> _source;
			readonly Func<TIn, string>          _key;

			public Source(ITable<string, object> store, Func<TIn, ValueTask<TOut>> source, Func<TIn, string> key)
			{
				_store  = store;
				_source = source;
				_key    = key;
			}

			public async ValueTask<TOut> Get(TIn parameter)
			{
				var key = _key(parameter);
				if (!_store.TryGet(key, out var result))
				{
					var source = await _source(parameter);
					_store.Assign(key, source!);
					return source;
				}

				return result.To<TOut>();
			}
		}
	}
}