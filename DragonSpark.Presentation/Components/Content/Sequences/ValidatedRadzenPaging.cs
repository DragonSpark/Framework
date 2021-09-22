using DragonSpark.Model.Results;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences
{
	sealed class ValidatedRadzenPaging<T> : IRadzenPaging<T>
	{
		readonly IRadzenPaging<T>                _previous;
		readonly IMutable<int?>                  _store;
		readonly IEqualityComparer<LoadDataArgs> _comparer;

		public ValidatedRadzenPaging(IRadzenPaging<T> previous)
			: this(previous, new Variable<int?>(), LoadDataArgsEqualityComparer.Default) {}

		public ValidatedRadzenPaging(IRadzenPaging<T> previous, IMutable<int?> store,
		                             IEqualityComparer<LoadDataArgs> comparer)
		{
			_previous = previous;
			_store    = store;
			_comparer = comparer;
		}

		public Task Get(LoadDataArgs parameter)
		{
			var current = _store.Get();
			var code    = _comparer.GetHashCode(parameter);
			if (!current.HasValue || !code.Equals(current.Value))
			{
				_store.Execute(code);
				return _previous.Get(parameter);
			}

			return Task.CompletedTask;
		}

		public ulong Count => _previous.Count;

		public IEnumerable<T>? Current => _previous.Current;
	}
}