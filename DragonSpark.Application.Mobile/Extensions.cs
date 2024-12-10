using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Mobile;

public static class Extensions
{
	public static BuildHostContext WithFrameworkConfigurations(this BuildHostContext @this)
		=> Configure.Default.Get(@this);
}
