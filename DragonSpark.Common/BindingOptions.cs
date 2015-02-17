using System.Reflection;

namespace DragonSpark.Common
{
	public static class BindingOptions
	{
		public static readonly BindingFlags
			AllProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
			PublicProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
	}
}
