using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;

namespace DragonSpark.Application.Hosting.Console;

[UsedImplicitly]
public sealed class ConsoleApplicationAttribute : HostingAttribute
{
	public ConsoleApplicationAttribute() : base(typeof(ConsoleApplicationAttribute).Assembly) {}
}