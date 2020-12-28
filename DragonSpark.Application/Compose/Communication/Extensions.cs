using DragonSpark.Compose;

namespace DragonSpark.Application.Compose.Communication
{
	public static class Extensions
	{
		public static ConfiguredApiContextRegistration<T> WithState<T>(this ConfiguredApiContextRegistration<T> @this)
			where T : class
			=> @this.Append(ApplyState.Default.Execute);
	}
}