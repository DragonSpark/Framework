using DragonSpark.Model.Sequences;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

sealed class DefaultAssemblies : Instances<Assembly>
{
	public static DefaultAssemblies Default { get; } = new DefaultAssemblies();

	DefaultAssemblies() : base(PrimaryAssembly.Default, HostingAssembly.Default) {}
}