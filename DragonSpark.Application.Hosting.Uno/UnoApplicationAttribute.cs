using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Uno;

public sealed class UnoApplicationAttribute : HostingAttribute
{
	public UnoApplicationAttribute() : base(A.Type<UnoApplicationAttribute>().Assembly) {}
}
