using System;

namespace DragonSpark.Application.Communication.Entity
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=false)]
	public sealed class TagAttribute : Attribute
	{
		readonly string tag;

		public TagAttribute( string tag )
		{
			this.tag = tag;
		}

		public string Tag
		{
			get { return tag; }
		}
	}
}