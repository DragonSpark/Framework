using System;

namespace DragonSpark.Windows.Entity
{
	[AttributeUsage( AttributeTargets.Property )]
	public class DefaultIncludeAttribute : Attribute
	{
		public string AlsoInclude { get; set; }
	}
}