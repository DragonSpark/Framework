using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Properties;
using System;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.State
{
	sealed class UpdateActivity : IUpdateActivity
	{
		public static UpdateActivity Default { get; } = new UpdateActivity();

		UpdateActivity() : this(Activities.Default.Get, IsActive.Default) {}

		readonly Func<object, ConcurrentBag<object>> _activities;
		readonly IProperty<object, bool>             _active;

		public UpdateActivity(Func<object, ConcurrentBag<object>> activities, IProperty<object, bool> active)
		{
			_activities = activities;
			_active     = active;
		}

		public void Execute(Pair<object, object> parameter)
		{
			var (key, value) = parameter;
			_active.Assign(key, true);
			_activities(key).Add(value);
		}

		public void Execute(object parameter)
		{
			var store = _activities(parameter);
			store.TryTake(out _);
			if (store.IsEmpty)
			{
				_active.Assign(parameter, false);
			}
		}
	}
}