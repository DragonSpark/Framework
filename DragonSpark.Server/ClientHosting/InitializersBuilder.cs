using System;

namespace DragonSpark.Server.ClientHosting
{
	public class InitializersBuilder : ClientModuleBuilder
	{
		public InitializersBuilder( string initialPath = "viewmodels" ) : base( initialPath )
		{}

		protected override bool IsResource( AssemblyResource resource )
		{
			var result = base.IsResource( resource ) && resource.ResourceName.EndsWith( ".initialize.js", StringComparison.InvariantCultureIgnoreCase );
			return result;
		}
	}
}