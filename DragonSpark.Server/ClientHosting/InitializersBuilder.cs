using System;

namespace DragonSpark.Server.ClientHosting
{
	public class InitializersBuilder : ClientModuleBuilder
	{
		public InitializersBuilder( string defaultParameter = "viewmodels" ) : base( defaultParameter )
		{}

		protected override bool IsResource( string parameter, AssemblyResource resource )
		{
			var result = base.IsResource( parameter, resource ) && resource.ResourceName.EndsWith( ".initialize.js", StringComparison.InvariantCultureIgnoreCase );
			return result;
		}
	}
}