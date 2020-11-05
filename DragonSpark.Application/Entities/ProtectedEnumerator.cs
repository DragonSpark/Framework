using AsyncUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValueTask = System.Threading.Tasks.ValueTask;

namespace DragonSpark.Application.Entities
{
	sealed class ProtectedEnumerator<T> : IAsyncEnumerator<T>
	{
		readonly IAsyncEnumerator<T> _previous;
		readonly AsyncLock           _lock;

		public ProtectedEnumerator(IAsyncEnumerator<T> previous, AsyncLock @lock)
		{
			_previous = previous;
			_lock     = @lock;
		}

		public ValueTask DisposeAsync() => _previous.DisposeAsync();

		public async ValueTask<bool> MoveNextAsync()
		{
			using (await _lock.LockAsync().ConfigureAwait(false))
			{
				return await _previous.MoveNextAsync().ConfigureAwait(false);
			}
		}

		public T Current => _previous.Current;
	}
}