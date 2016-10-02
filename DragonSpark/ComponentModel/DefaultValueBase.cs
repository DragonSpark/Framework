using System;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public abstract class DefaultValueBase : HostingAttributeBase
	{
		protected DefaultValueBase( Func<object, IDefaultValueProvider> provider ) : base( provider ) {}
	}
}