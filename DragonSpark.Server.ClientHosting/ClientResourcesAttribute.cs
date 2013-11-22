using System;

namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public class ClientResourcesAttribute : Attribute
	{
		readonly string name;

		public ClientResourcesAttribute( string name )
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}
	}
}