using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Server.ClientHosting
{
	[ContentProperty( "Modules" )]
	public class WidgetBuilderOptions
	{
		public WidgetBuilderOptions()
		{
			InitialPath = "widgets";
		}

		public string InitialPath { get; set; }

		public override string ToString()
		{
			return InitialPath;
		}

		public List<WidgetModule> Modules
		{
			get { return modules ?? ( modules = new List<WidgetModule>() ); }
		}	List<WidgetModule> modules;
	}

	public class WidgetsBuilder : ClientModuleBuilder<WidgetModule, WidgetBuilderOptions>
	{
		public WidgetsBuilder( WidgetBuilderOptions options ) : base( options )
		{}

		protected override bool IsResource( WidgetBuilderOptions parameter, AssemblyResource resource )
		{
			var result = base.IsResource( parameter, resource ) && resource.ResourceName.EndsWith( ".initialize.js", StringComparison.InvariantCultureIgnoreCase );
			return result;
		}

		protected override WidgetModule Create( WidgetBuilderOptions parameter, AssemblyResource resource )
		{
			var path = CreatePath( resource ).Replace( "dragonspark/Widgets", "widgets" );
			var name = new DirectoryInfo( path ).Parent.Name;
			var item = parameter.Modules.FirstOrDefault( x => x.Name == name ) ?? new WidgetModule { Name = name };

			var result = item.With( x => x.Path = x.Path ?? path );
			return result;
		}
	}
}