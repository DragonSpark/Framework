using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	sealed class ActivityAwareOperation : IOperation
	{
		readonly IOperation      _operation;
		readonly object          _owner;
		readonly IUpdateActivity _activity;

		public ActivityAwareOperation(IOperation operation, object owner)
			: this(operation, owner, UpdateActivity.Default) {}

		public ActivityAwareOperation(IOperation operation, object owner, IUpdateActivity activity)
		{
			_operation = operation;
			_owner     = owner;
			_activity  = activity;
		}

		public async ValueTask Get()
		{
			_activity.Assign(_owner, _operation);
			try
			{
				await _operation.Await();
			}
			finally
			{
				_activity.Execute(_owner);
			}
		}
	}

	sealed class ActivityAwareOperation<T> : IOperation<T>
	{
		readonly IOperation<T>   _operation;
		readonly object          _owner;
		readonly IUpdateActivity _activity;

		public ActivityAwareOperation(IOperation<T> operation, object owner)
			: this(operation, owner, UpdateActivity.Default) {}

		public ActivityAwareOperation(IOperation<T> operation, object owner, IUpdateActivity activity)
		{
			_operation = operation;
			_owner     = owner;
			_activity  = activity;
		}

		public async ValueTask Get(T parameter)
		{
			_activity.Assign(_owner, _operation);
			try
			{
				await _operation.Await(parameter);
			}
			finally
			{
				_activity.Execute(_owner);
			}
		}
	}
}