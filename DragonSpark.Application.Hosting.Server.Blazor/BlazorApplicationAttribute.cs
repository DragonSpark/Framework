using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	public sealed class BlazorApplicationAttribute : HostingAttribute
	{
		public BlazorApplicationAttribute() : base(A.Type<BlazorApplicationAttribute>().Assembly) {}
	}
}