using System;

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
			var result = new WidgetModule { Name = resource.ResourceName, Path = CreatePath( resource ) };
			return result;
		}
	}
}