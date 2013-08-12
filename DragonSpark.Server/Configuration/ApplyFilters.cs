using System.Collections.ObjectModel;
using System.Web.Http;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Filters" )]
	public class ApplyFilters : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			Filters.Apply( configuration.Filters.Add );
		}

		public Collection<System.Web.Http.Filters.IFilter> Filters
		{
			get { return filters; }
		}	readonly Collection<System.Web.Http.Filters.IFilter> filters = new Collection<System.Web.Http.Filters.IFilter>();
	}
}