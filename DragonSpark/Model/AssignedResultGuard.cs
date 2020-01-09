using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model
{
	sealed class AssignedEntryGuard<T> : AssignedGuard<T, ArgumentNullException>
	{
		public static AssignedEntryGuard<T> Default { get; } = new AssignedEntryGuard<T>();

		AssignedEntryGuard() : this(AssignedArgumentMessage.Default.Then().Out<T>()) {}

		public AssignedEntryGuard(Func<T, string> message) : base(message) {}
	}

	sealed class AssignedResultGuard<T> : AssignedGuard<T, InvalidOperationException>
	{
		public static AssignedResultGuard<T> Default { get; } = new AssignedResultGuard<T>();

		AssignedResultGuard() : base(AssignedResultMessage.Default.Then().Out<T>()) {}

		public AssignedResultGuard(ICondition<T> condition, Func<T, string> message) : base(condition, message) {}
	}

	public class AssignedGuard<T, TException> : Guard<T, TException> where TException : Exception
	{
		public AssignedGuard(Func<T, string> message) : this(Is.Assigned<T>().Inverse().Out(), message) {}

		public AssignedGuard(ICondition<T> condition, Func<T, string> message) : base(condition, message) {}
	}
}