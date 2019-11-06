using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.xUnit
{
	public sealed class XunitTestingApplicationAttribute : HostingAttribute
	{
		public XunitTestingApplicationAttribute() : base(typeof(XunitTestingApplicationAttribute).Assembly) {}
	}
}