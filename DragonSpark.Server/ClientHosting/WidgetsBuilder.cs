using DragonSpark.Extensions;
using System;
using System.IO;

namespace DragonSpark.Server.ClientHosting
{
	public class WidgetsBuilder : ClientModuleBuilder<WidgetModule>
	{
		public WidgetsBuilder( string initialPath = "widgets" ) : base( initialPath )
		{}

		protected override bool IsResource( AssemblyResource resource )
		{
			var result = base.IsResource( resource ) && resource.ResourceName.EndsWith( ".initialize.js", StringComparison.InvariantCultureIgnoreCase );
			return result;
		}

		protected override WidgetModule Create( AssemblyResource resource )
		{
			return new WidgetModule
			{
				Name = resource.ResourceName,
				Path = string.Concat( resource.Assembly.FromMetadata<ClientResourcesAttribute, string>( x => x.Name ), Path.AltDirectorySeparatorChar, Path.GetFileNameWithoutExtension( resource.ResourceName ) )
			};
		}
	}
}