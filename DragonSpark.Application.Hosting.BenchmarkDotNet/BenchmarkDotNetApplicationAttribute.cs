using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	public sealed class BenchmarkDotNetApplicationAttribute : HostingAttribute
	{
		public BenchmarkDotNetApplicationAttribute() : base(typeof(BenchmarkDotNetApplicationAttribute).Assembly) {}
	}
}