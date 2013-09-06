using System.IO;

namespace DragonSpark.Server.ClientHosting
{
	public class CommandsBuilder : ClientModuleBuilder
	{
		public CommandsBuilder( string defaultParameter = "commands" ) : base( defaultParameter )
		{}

		protected override bool IsResource( string parameter, AssemblyResource resource )
		{
			var result = base.IsResource( parameter, resource ) && Path.GetExtension( resource.ResourceName ) == ".js";
			return result;
		}
	}
}