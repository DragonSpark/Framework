using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Runtime
{
	sealed class AssignedEntryGuard<T> : AssignedGuard<T, ArgumentNullException>
	{
		public static AssignedEntryGuard<T> Default { get; } = new AssignedEntryGuard<T>();

		AssignedEntryGuard() : this(AssignedArgumentMessage.Default.Then().Bind<T>().Get()) {}

		public AssignedEntryGuard(ISelect<T, string> message) : base(message) {}
	}

	sealed class AssignedResultGuard<T> : AssignedGuard<T, InvalidOperationException>
	{
		/*public static AssignedResultGuard<T> Default { get; } = new AssignedResultGuard<T>();

		AssignedResultGuard() : this(AssignedResultMessage.Default.Then().Bind<T>().Get()) {}*/

		public AssignedResultGuard(ISelect<T, string> message) : base(message) {}
	}

	public class AssignedGuard<T, TException> : Guard<T, TException> where TException : Exception
	{
		readonly static ICondition<T> Condition = IsAssigned<T>.Default.Then().Inverse().Out();

		public AssignedGuard(ISelect<T, string> message) : base(Condition, message) {}
	}
}