using System.Collections.Generic;
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
}