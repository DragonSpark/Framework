using DragonSpark.Server.Application;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	sealed class BlazorServerProfile : ServerProfile
	{
		public static BlazorServerProfile Default { get; } = new BlazorServerProfile();

		BlazorServerProfile() : base(DefaultServiceConfiguration.Default.Execute,
		                             DefaultApplicationConfiguration.Default.Execute) {}
	}
}