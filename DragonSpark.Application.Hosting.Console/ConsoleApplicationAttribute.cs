using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Console
{
	public sealed class ConsoleApplicationAttribute : HostingAttribute
	{
		public ConsoleApplicationAttribute() : base(typeof(ConsoleApplicationAttribute).Assembly) {}
	}
}