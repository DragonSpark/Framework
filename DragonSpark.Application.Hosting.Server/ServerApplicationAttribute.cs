using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Server
{
	public sealed class ServerApplicationAttribute : HostingAttribute
	{
		public ServerApplicationAttribute() : base(A.Type<ServerApplicationAttribute>().Assembly) {}
	}
}