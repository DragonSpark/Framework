using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	public struct CompositeActivatorParameters
	{
		public CompositeActivatorParameters( LifetimeContext context, CompositionOperation operation )
		{
			Context = context;
			Operation = operation;
		}

		public LifetimeContext Context { get; }
		public CompositionOperation Operation { get; }
	}
}