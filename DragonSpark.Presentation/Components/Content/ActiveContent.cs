using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public class ActiveContent<T> : IActiveContent<T>
	{
		readonly Func<ValueTask<T>>       _content;
		readonly IMutable<ValueTuple<T>?> _store;

		public ActiveContent(Func<ValueTask<T>> content) : this(new Variable<ValueTuple<T>?>(), content) {}

		public ActiveContent(IMutable<ValueTuple<T>?> store, Func<ValueTask<T>> content)
		{
			_store   = store;
			_content = content;
		}

		public override string? ToString() => _store.Get()?.Item1?.ToString();

		public bool HasValue => _store.Get().HasValue;

		public async ValueTask<T> Get()
		{
			var store = _store.Get();
			if (store is null)
			{
				var result = await _content();
				_store.Execute(ValueTuple.Create(result));
				return result;
			}

			return store.Value.Item1;
		}
	}
}