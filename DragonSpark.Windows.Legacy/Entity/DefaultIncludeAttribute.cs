using System;

namespace DragonSpark.Windows.Legacy.Entity
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class DefaultIncludeAttribute : Attribute
	{
		public string AlsoInclude { get; set; }
	}
}