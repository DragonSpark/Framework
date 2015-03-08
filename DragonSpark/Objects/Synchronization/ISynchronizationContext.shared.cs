using System;

namespace DragonSpark.Objects.Synchronization
{
	public interface ISynchronizationContext
	{
		string FirstExpression { get; }
		string SecondExpression { get; }
		Type TypeConverterType { get; }

		ISynchronizationContext CreateMirror();

		void Synchronize( SynchronizationContainerContext context );
	}
}