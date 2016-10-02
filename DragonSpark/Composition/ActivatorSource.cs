using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	sealed class ActivatorSource : SuppliedSource<ActivationParameter, object>
	{
		readonly IStackSource<CompositeActivatorParameters> stack;
		
		public ActivatorSource( IStackSource<CompositeActivatorParameters> stack, Func<ActivationParameter, object> factory, ActivationParameter parameter ) : base( factory, parameter )
		{
			this.stack = stack;
		}

		public object Create( LifetimeContext context, CompositionOperation operation )
		{
			using ( stack.Assignment( new CompositeActivatorParameters( context, operation ) ) )
			{
				return Get();
			}
		}
	}
}