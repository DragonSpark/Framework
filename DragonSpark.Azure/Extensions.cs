using DragonSpark.Composition.Compose;

namespace DragonSpark.Azure
{
	public static class Extensions
	{
		public static BuildHostContext WithAzureConfigurations(this BuildHostContext @this)
			=> Configure.Default.Get(@this);
	}
}
