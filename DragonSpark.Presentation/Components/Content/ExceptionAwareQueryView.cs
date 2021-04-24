using DragonSpark.Model.Operations;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class ExceptionAwareQueryView<T> : IQueryView<T>
	{
		readonly IQueryView<T>            _previous;
		readonly IAllocated<LoadDataArgs> _handle;

		public ExceptionAwareQueryView(IQueryView<T> previous, Func<Exception, Task> handle)
			: this(previous, new ExceptionAwareLoadOperation(previous, handle)) {}

		public ExceptionAwareQueryView(IQueryView<T> previous, IAllocated<LoadDataArgs> handle)
		{
			_previous = previous;
			_handle   = handle;
		}

		public Task Get(LoadDataArgs parameter) => _handle.Get(parameter);

		public IEnumerable<T> Current => _previous.Current;

		public ulong Count => _previous.Count;
	}
}