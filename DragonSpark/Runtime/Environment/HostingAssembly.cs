using System.Reflection;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Environment
{
	public sealed class HostingAssembly : Result<Assembly>
	{
		public static HostingAssembly Default { get; } = new HostingAssembly();

		HostingAssembly() : base(PrimaryAssembly.Default.Select(x => x.Attribute<HostingAttribute>().Get())) {}
	}
}