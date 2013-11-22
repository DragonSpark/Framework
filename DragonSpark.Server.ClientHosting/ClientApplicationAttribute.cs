using System;

namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public class ClientApplicationAttribute : ClientResourcesAttribute
	{
		public ClientApplicationAttribute() : base( "application" )
		{}
	}
}
