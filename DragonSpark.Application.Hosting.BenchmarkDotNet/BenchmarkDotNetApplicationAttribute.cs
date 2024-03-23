using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet;

[UsedImplicitly]
public sealed class BenchmarkDotNetApplicationAttribute : HostingAttribute
{
	public BenchmarkDotNetApplicationAttribute() : base(typeof(BenchmarkDotNetApplicationAttribute).Assembly) {}
}