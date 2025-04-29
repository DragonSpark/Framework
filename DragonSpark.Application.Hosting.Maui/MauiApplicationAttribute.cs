using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Maui;

public sealed class MauiApplicationAttribute : HostingAttribute
{
	public MauiApplicationAttribute() : base(A.Type<MauiApplicationAttribute>().Assembly) {}
}
