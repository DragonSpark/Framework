using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Aspire;

public sealed class AspireHostApplicationAttribute : HostingAttribute
{
	public AspireHostApplicationAttribute() : base(A.Type<AspireHostApplicationAttribute>().Assembly) {}
}