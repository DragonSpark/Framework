using System;

namespace DragonSpark.Windows.Entity
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class DefaultIncludeAttribute : Attribute
	{
		public string AlsoInclude { get; set; }
	}
}