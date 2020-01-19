using DragonSpark.Model.Sequences;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class DefaultAssemblies : ArrayInstance<Assembly>
	{
		public static DefaultAssemblies Default { get; } = new DefaultAssemblies();

		DefaultAssemblies() : base(PrimaryAssembly.Default, HostingAssembly.Default) {}
	}
}