using System;

namespace DragonSpark.Server.Legacy.Entity
{
	[AttributeUsage( AttributeTargets.Property )]
	public class DefaultIncludeAttribute : Attribute
	{
		public string AlsoInclude { get; set; }
	}
}