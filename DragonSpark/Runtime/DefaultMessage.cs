using DragonSpark.Compose;
using DragonSpark.Text;
using System;

namespace DragonSpark.Runtime
{
	sealed class DefaultMessage<T> : Message<Type>
	{
		public static DefaultMessage<T> Default { get; } = new DefaultMessage<T>();

		DefaultMessage()
			: base(x => $"Expected instance of type {A.Type<T>()} to be assigned, but an operation using an instance of {x} did not produce this.") {}
	}

	sealed class DefaultMessage : Message<Type>
	{
		public static DefaultMessage Default { get; } = new DefaultMessage();

		DefaultMessage()
			: base(x => $"Expected instance of type {x} to be assigned, but the provided instance was not.") {}
	}
}