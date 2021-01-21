using DragonSpark.Compose;
using DragonSpark.Model;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	public readonly struct ActivityContext : IAsyncDisposable
	{
		readonly static object Instance = new();

		readonly IUpdateActivityReceiver _receiver;
		readonly object?                 _subject;

		public ActivityContext(object? subject) : this(UpdateActivityReceiver.Default, subject) {}

		public ActivityContext(IUpdateActivityReceiver receiver, object? subject)
		{
			_receiver = receiver;
			_subject  = subject;
		}

		public async ValueTask<ActivityContext> Start()
		{
			if (_subject != null)
			{
				var task = _receiver.Get(Pairs.Create(_subject, Instance));
				if (!task.IsCompleted)
				{
					await task;
				}
			}
			return this;
		}

		public ValueTask DisposeAsync() => _subject != null ? _receiver.Get(_subject) : Task.CompletedTask.ToOperation();
	}
}