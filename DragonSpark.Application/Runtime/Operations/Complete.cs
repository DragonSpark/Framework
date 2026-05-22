using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

sealed class Complete<T> : ICompleted
{
	readonly ICompleted<T>  _completed;
	readonly IResulting<T?> _previous;
	readonly Loaded<T>      _loaded;

	public Complete(IResulting<T?> previous, ICompleted<T> completed, Task<T?> subject)
		: this(previous, completed, new Loaded<T>(subject)) {}

	public Complete(IResulting<T?> previous, ICompleted<T> completed, Loaded<T> loaded)
	{
		_previous  = previous;
		_completed = completed;
		_loaded    = loaded;
	}

	public Task Get()
	{
		if (_completed.Get(_previous))
		{
			var task = _loaded.Get();
			if (task is not null)
			{
				return _completed.Get(task.ToOperation());
			}
		}
		else
		{
			Dispose();
		}


		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_loaded.Dispose();
	}
}