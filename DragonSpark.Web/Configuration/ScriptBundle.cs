namespace DragonSpark.Web.Configuration
{
	public class ScriptBundle : Bundle
	{
		protected override System.Web.Optimization.Bundle CreateInstance()
		{
			var result = new System.Web.Optimization.ScriptBundle( Path );
			return result;
		}
	}
}