using DragonSpark.Extensions;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Web.Mvc;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Filters" )]
	public class ApplyGlobalFilters : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			Filters.Apply( GlobalFilters.Filters.Add );
		}

		public Collection<object> Filters
		{
			get { return filters; }
		}	readonly Collection<object> filters = new Collection<object>();
	}
}