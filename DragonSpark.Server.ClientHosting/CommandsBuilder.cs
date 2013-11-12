using System.IO;

namespace DragonSpark.Server.ClientHosting
{
	public class CommandsBuilder : ClientModuleBuilder
	{
		protected override bool IsResource( string parameter, AssemblyResource resource )
		{
			var result = IsResource( resource, "commands" ) && Path.GetExtension( resource.ResourceName ) == ".js";
			return result;
		}
	}
}