using System;

namespace DragonSpark.Runtime
{
	sealed class AssignedArgumentGuard<T> : AssignedGuard<T, ArgumentNullException>
	{
		public static AssignedArgumentGuard<T> Default { get; } = new AssignedArgumentGuard<T>();

		AssignedArgumentGuard() : base(x => $"Argument of type {x} was not assigned.") {}
	}
}