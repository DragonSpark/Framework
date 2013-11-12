using System.IO;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Server.ClientHosting
{
	public class WidgetsBuilder : ClientModuleBuilder<WidgetModule, WidgetBuilderOptions>
	{
		protected override bool IsResource( WidgetBuilderOptions parameter, AssemblyResource resource )
		{
			var result = IsResource( resource, "widgets" ) && resource.ResourceName.EndsWith( "viewmodel.js" );
			return result;
		}

		protected override WidgetModule Create( WidgetBuilderOptions parameter, AssemblyResource resource )
		{
			var input = parameter ?? new WidgetBuilderOptions();
			var path = CreatePath( resource );
			var name = new DirectoryInfo( path ).Parent.Name;
			var item = input.Overrides.FirstOrDefault( x => x.Name == name ) ?? new WidgetModule { Name = name };

			var result = item.With( x =>
			{
				x.Path = x.Path ?? path;
				x.ViewId = x.ViewId ?? x.Path.Replace( "/viewmodel", "/view" );
			} );
			return result;
		}
	}
}