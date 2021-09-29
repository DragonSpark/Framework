using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class Worker<T> : IAllocated
	{
		readonly IResulting<T?>           _content;
		readonly TaskCompletionSource<T?> _source;

		public Worker(IResulting<T?> content, TaskCompletionSource<T?> source)
		{
			_content = content;
			_source  = source;
		}

		public async Task Get()
		{
			try
			{
				var content = await _content.Await();
				_source.SetResult(content);
			}
			// ReSharper disable once CatchAllClause
			catch (Exception e)
			{
				_source.SetException(e);
			}
		}
	}
}