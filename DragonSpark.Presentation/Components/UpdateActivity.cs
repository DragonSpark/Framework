using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Properties;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Presentation.Compose;
using System;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components
{
	sealed class UpdateActivity : IUpdateActivity
	{
		public static UpdateActivity Default { get; } = new UpdateActivity();

		UpdateActivity() : this(Activities.Default.Get, ReceiverAwareProperty.Default) {}

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

	sealed class ReceiverAwareProperty : IProperty<object, bool>
	{
		public static ReceiverAwareProperty Default { get; } = new ReceiverAwareProperty();

		ReceiverAwareProperty() : this(IsActive.Default) {}

		readonly IProperty<object, bool> _previous;
		readonly ITable<object, Action>  _receivers;

		public ReceiverAwareProperty(IProperty<object, bool> previous) : this(previous, Receivers.Default) {}

		public ReceiverAwareProperty(IProperty<object, bool> previous, ITable<object, Action> receivers)
		{
			_previous  = previous;
			_receivers = receivers;
		}

		public ICondition<object> Condition => _previous.Condition;

		public bool Get(object parameter) => _previous.Get(parameter);

		public void Execute(Pair<object, bool> parameter)
		{
			var exists = _receivers.TryGet(parameter.Key, out var action);
			var target = exists ? action.Target.Verify().Paired(parameter.Value) : parameter;

			_previous.Execute(target);

			if (exists)
			{
				action();
			}
		}
	}
}