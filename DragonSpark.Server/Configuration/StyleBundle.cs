namespace DragonSpark.Server.Configuration
{
	public class StyleBundle : Bundle
	{
		protected override System.Web.Optimization.Bundle CreateInstance()
		{
			var result = new System.Web.Optimization.StyleBundle( Path );
			return result;
		}
	}
}