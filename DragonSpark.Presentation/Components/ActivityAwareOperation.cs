using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Properties;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	sealed class ActivityAwareOperation : IOperation
	{
		readonly IOperation              _operation;
		readonly object                  _owner;
		readonly IProperty<object, bool> _active;

		public ActivityAwareOperation(IOperation operation, object owner)
			: this(operation, owner, IsActive.Default) {}

		public ActivityAwareOperation(IOperation operation, object owner, IProperty<object, bool> active)
		{
			_operation = operation;
			_owner     = owner;
			_active    = active;
		}

		public async ValueTask Get()
		{
			if (!_active.Get(_owner))
			{
				_active.Assign(_owner, true);
				try
				{
					await _operation.Get();
				}
				finally
				{
					_active.Assign(_owner, false);
				}
			}
		}
	}

	sealed class ActivityAwareOperation<T> : IOperation<T>
	{
		readonly IOperation<T>           _operation;
		readonly object                  _owner;
		readonly IProperty<object, bool> _active;

		public ActivityAwareOperation(IOperation<T> operation, object owner)
			: this(operation, owner, IsActive.Default) {}

		public ActivityAwareOperation(IOperation<T> operation, object owner, IProperty<object, bool> active)
		{
			_operation = operation;
			_owner     = owner;
			_active    = active;
		}

		public async ValueTask Get(T parameter)
		{
			if (!_active.Get(_owner))
			{
				_active.Assign(_owner, true);
				try
				{
					await _operation.Get(parameter);
				}
				finally
				{
					_active.Assign(_owner, false);
				}
			}
		}
	}

}