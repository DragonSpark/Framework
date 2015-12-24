using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC
{
	public class BuildChainMonitorStrategy : BuilderStrategy
	{
		class Clear : IRequiresRecovery
		{
			readonly Stack<NamedTypeBuildKey> stack;

			public Clear( Stack<NamedTypeBuildKey> stack )
			{
				this.stack = stack;
			}

			public void Recover()
			{
				stack.Clear();
			}
		}

		public override void PreBuildUp( IBuilderContext context )
		{
			var stack = new Chain( context.Strategies ).Item;

			if ( !stack.Any() )
			{
				context.RecoveryStack.Add( new Clear( stack ) );
			}

			stack.Push( context.OriginalBuildKey );
		}

		public override void PostBuildUp( IBuilderContext context )
		{
			new Chain( context.Strategies ).Item.Pop();
		}
	}
}