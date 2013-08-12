namespace DragonSpark.Server.Configuration
{
	public abstract class BundleDirective
	{
		internal abstract System.Web.Optimization.Bundle Apply( System.Web.Optimization.Bundle bundle );
	}
}