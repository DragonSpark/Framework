using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class AllocatedWorker : IAllocated
	{
		readonly Task                 _content;
		readonly TaskCompletionSource _source;

		public AllocatedWorker(Task content, TaskCompletionSource source)
		{
			_content = content;
			_source  = source;
		}

		public async Task Get()
		{
			try
			{
				await _content.ConfigureAwait(false);
				_source.SetResult();
			}
			// ReSharper disable once CatchAllClause
			catch (Exception e)
			{
				_source.SetException(e);
			}
		}
	}
}