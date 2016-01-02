using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public static class BindingOptions
	{
		public const BindingFlags AllProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
	}
}
