using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model
{
	sealed class AssignedEntryGuard<T> : AssignedGuard<T, ArgumentNullException>
	{
		public static AssignedEntryGuard<T> Default { get; } = new AssignedEntryGuard<T>();

		AssignedEntryGuard() : this(AssignedArgumentMessage.Default.Then().Bind<T>().Get()) {}

		public AssignedEntryGuard(ISelect<T, string> message) : base(message) {}
	}

	sealed class AssignedResultGuard<T> : AssignedGuard<T, InvalidOperationException>
	{
		public static AssignedResultGuard<T> Default { get; } = new AssignedResultGuard<T>();

		AssignedResultGuard() : base(AssignedResultMessage.Default.Then().Bind<T>().Get()) {}

		public AssignedResultGuard(ICondition<T> condition, ISelect<T, string> message) : base(condition, message) {}
	}

	public class AssignedGuard<T, TException> : Guard<T, TException> where TException : Exception
	{
		public AssignedGuard(ISelect<T, string> message) : this(Is.Assigned<T>().Then().Inverse().Out(), message) {}

		public AssignedGuard(ICondition<T> condition, ISelect<T, string> message) : base(condition, message) {}
	}
}