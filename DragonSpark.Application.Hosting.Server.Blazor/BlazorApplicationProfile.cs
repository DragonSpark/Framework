using DragonSpark.Application.Compose;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	sealed class BlazorApplicationProfile : ApplicationProfile
	{
		public static BlazorApplicationProfile Default { get; } = new BlazorApplicationProfile();

		BlazorApplicationProfile() : base(DefaultServiceConfiguration.Default.Execute,
		                             DefaultApplicationConfiguration.Default.Execute) {}
	}
}