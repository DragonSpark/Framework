using System;

namespace DragonSpark.Server.ClientHosting
{
	public class InitializersBuilder : ClientModuleBuilder
	{
		protected override bool IsResource( string parameter, AssemblyResource resource )
		{
			var result = resource.ResourceName.EndsWith( ".initialize.js", StringComparison.InvariantCultureIgnoreCase );
			return result;
		}
	}
}