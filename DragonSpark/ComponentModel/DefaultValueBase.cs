using DragonSpark.TypeSystem;
using System;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public abstract class DefaultValueBase : HostingAttribute
	{
		protected DefaultValueBase( Func<IDefaultValueProvider> provider ) : base( provider )
		{}
	}
}