using DragonSpark.Compose;
using System;

namespace DragonSpark.Model
{
	sealed class AssignedEntryGuard<T> : AssignedGuard<T, ArgumentNullException>
	{
		public static AssignedEntryGuard<T> Default { get; } = new AssignedEntryGuard<T>();

		AssignedEntryGuard() : this(AssignedArgumentMessage.Default.Then().Out<T>()) {}

		public AssignedEntryGuard(Func<T, string> message) : base(message) {}
	}
}