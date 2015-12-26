using DragonSpark.Runtime.Values;
using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;

namespace DragonSpark.Activation.IoC
{
	class Chain : ConnectedValue<Stack<NamedTypeBuildKey>>
	{
		public Chain( IStrategyChain instance ) : base( instance, typeof(Chain), () => new Stack<NamedTypeBuildKey>() )
		{ }
	}
}