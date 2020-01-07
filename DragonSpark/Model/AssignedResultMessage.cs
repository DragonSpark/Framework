using DragonSpark.Text;
using System;

namespace DragonSpark.Model
{
	sealed class AssignedArgumentMessage : Message<Type>
	{
		public static AssignedArgumentMessage Default { get; } = new AssignedArgumentMessage();

		AssignedArgumentMessage()
			: base(x => $"Expected an argument of type {x} to be assigned, but the provided instance was not.") {}
	}

	sealed class AssignedResultMessage : Message<Type>
	{
		public static AssignedResultMessage Default { get; } = new AssignedResultMessage();

		AssignedResultMessage()
			: base(x => $"Expected an result of type {x} to be assigned, but the resulting instance was not.") {}
	}
}