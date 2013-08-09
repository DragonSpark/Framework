using DragonSpark.Extensions;

namespace DragonSpark.Web.Configuration
{
	public class BundleFile  : BundleDirective
	{
		public string File { get; set; }

		internal override System.Web.Optimization.Bundle Apply( System.Web.Optimization.Bundle bundle )
		{
			return bundle.Include( File.ToStringArray() );
		}
	}
}