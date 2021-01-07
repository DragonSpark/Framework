using AsyncUtilities;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Entities
{
	public class LockInstance<T> : DragonSpark.Model.Results.Instance<AsyncLock> where T : class
	{
		protected LockInstance(T context) : this(context, Locks.Default.Get) {}

		protected LockInstance(T context, Func<T, AsyncLock> locks) : base(locks(context)) {}
	}
}