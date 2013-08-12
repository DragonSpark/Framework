using DragonSpark.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Optimization;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Bundles" )]
	public class BundlesConfigurator : IHttpApplicationConfigurator
	{
		public IgnoreDirectiveCollection Ignore
		{
			get { return ignore; }
		}	readonly IgnoreDirectiveCollection ignore = new IgnoreDirectiveCollection();

		public Collection<Bundle> Bundles
		{
			get { return bundles; }
		}	readonly Collection<Bundle> bundles = new Collection<Bundle>();

		void IHttpApplicationConfigurator.Configure( HttpConfiguration configuration )
		{
			var table = BundleTable.Bundles;
			Ignore.ClearList.IsTrue(table.IgnoreList.Clear);
			Ignore.Apply( y => table.IgnoreList.Ignore( y.Item, y.Mode ) );
			
			Bundles.Select( y => y.Create() ).ToArray().Apply( table.Add );
		}
	}
}