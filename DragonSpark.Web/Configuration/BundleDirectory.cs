namespace DragonSpark.Web.Configuration
{
	public class BundleDirectory : BundleDirective
	{
		public string Directory { get; set; }

		public string Search { get; set; }

		public bool IncludeSubdirectories { get; set; }

		internal override System.Web.Optimization.Bundle Apply( System.Web.Optimization.Bundle bundle )
		{
			return bundle.IncludeDirectory( Directory, Search, IncludeSubdirectories );
		}
	}
}