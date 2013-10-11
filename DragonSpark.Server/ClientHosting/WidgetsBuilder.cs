using DragonSpark.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Server.ClientHosting
{
	[ContentProperty( "Overrides" )]
	public class WidgetBuilderOptions
	{
		public List<WidgetModule> Overrides
		{
			get { return modules ?? ( modules = new List<WidgetModule>() ); }
		}	List<WidgetModule> modules;
	}

	public class WidgetsBuilder : ClientModuleBuilder<WidgetModule, WidgetBuilderOptions>
	{
		protected override bool IsResource( WidgetBuilderOptions parameter, AssemblyResource resource )
		{
			var result = IsResource( resource, "widgets" ) && resource.ResourceName.EndsWith( "viewmodel.js" );
			return result;
		}

		protected override WidgetModule Create( WidgetBuilderOptions parameter, AssemblyResource resource )
		{
			var path = CreatePath( resource );
			var name = new DirectoryInfo( path ).Parent.Name;
			var item = parameter.Overrides.FirstOrDefault( x => x.Name == name ) ?? new WidgetModule { Name = name };

			var result = item.With( x =>
			{
				x.Path = x.Path ?? path;
				x.ViewId = x.ViewId ?? x.Path.Replace( "/viewmodel", "/view" );
			} );
			return result;
		}
	}
}