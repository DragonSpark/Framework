using System.IO;

namespace DragonSpark.Server.ClientHosting
{
	public class CommandsBuilder : ClientModuleBuilder
	{
		public CommandsBuilder( string initialPath = "commands" ) : base( initialPath )
		{}

		protected override bool IsResource( AssemblyResource resource )
		{
			var result = base.IsResource( resource ) && Path.GetExtension( resource.ResourceName ) == ".js";
			return result;
		}
	}
}