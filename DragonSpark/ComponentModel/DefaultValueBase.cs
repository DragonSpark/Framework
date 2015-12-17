using System;
using DragonSpark.Aspects;
using DragonSpark.TypeSystem;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public abstract class DefaultValueBase : SurrogateAttribute
	{
		protected DefaultValueBase( [OfType( typeof(IDefaultValueProvider) )]Type @for ) : base( @for )
		{}
	}
}