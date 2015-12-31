using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public static class BindingOptions
	{
		readonly public static BindingFlags
			AllProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
			PublicProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
	}
}
