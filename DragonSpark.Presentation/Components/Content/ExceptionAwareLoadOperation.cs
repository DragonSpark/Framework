using DragonSpark.Model.Operations;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class ExceptionAwareLoadOperation : IAllocated<LoadDataArgs>
	{
		readonly IAllocated<LoadDataArgs> _previous;
		readonly Func<Exception, Task>    _handle;

		public ExceptionAwareLoadOperation(IAllocated<LoadDataArgs> previous, Func<Exception, Task> handle)
		{
			_previous = previous;
			_handle   = handle;
		}

		public async Task Get(LoadDataArgs parameter)
		{
			try
			{
				await _previous.Get(parameter);
			}
			catch (Exception e)
			{
				await _handle(e);
			}
		}
	}
}