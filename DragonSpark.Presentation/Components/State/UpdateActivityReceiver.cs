using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	sealed class UpdateActivityReceiver : IUpdateActivityReceiver
	{
		public static UpdateActivityReceiver Default { get; } = new UpdateActivityReceiver();

		UpdateActivityReceiver() : this(UpdateActivity.Default) {}

		readonly IUpdateActivity       _activity;
		readonly ISelect<object, bool> _active;

		public UpdateActivityReceiver(IUpdateActivity activity) : this(activity, IsActive.Default) {}

		public UpdateActivityReceiver(IUpdateActivity activity, ISelect<object, bool> active)
		{
			_activity = activity;
			_active   = active;
		}

		public ValueTask Get(Pair<object, object> parameter)
		{
			var (key, _) = parameter;

			var start = !_active.Get(key);

			_activity.Execute(parameter);

			var result = start && key is IActivityReceiver receiver ? receiver.Start() : ValueTask.CompletedTask;
			return result;
		}

		public ValueTask Get(object parameter)
		{
			_activity.Execute(parameter);

			var completed = !_active.Get(parameter);
			var result = completed && parameter is IActivityReceiver receiver
				             ? receiver.Complete()
				             : ValueTask.CompletedTask;
			return result;
		}
	}
}