using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	public sealed class WebJobApplicationAttribute : HostingAttribute
	{
		public WebJobApplicationAttribute() : base(typeof(WebJobApplicationAttribute).Assembly) {}
	}
}