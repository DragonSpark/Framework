using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public static class DragonSparkBindingOptions
	{
		public static readonly BindingFlags
			AllProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
			PublicProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
	}
}