using System;

namespace DragonSpark.Application.Client.Eventing
{
	[AttributeUsage( AttributeTargets.Class )]
	public class MessageNameAttribute : Attribute
	{
		readonly string name;

		public MessageNameAttribute( string name )
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}
	}
}