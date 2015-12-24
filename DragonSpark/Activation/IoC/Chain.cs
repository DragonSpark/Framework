using System.Collections.Generic;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC
{
	class Chain : ConnectedValue<Stack<NamedTypeBuildKey>>
	{
		public Chain( IStrategyChain instance ) : base( instance, typeof( Chain ), () => new Stack<NamedTypeBuildKey>() )
		{ }
	}
}