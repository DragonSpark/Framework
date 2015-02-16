using System;

namespace DragonSpark.Server.Entity
{
	[AttributeUsage( AttributeTargets.Property )]
	public class DefaultIncludeAttribute : Attribute
	{
		public string AlsoInclude { get; set; }
	}
}