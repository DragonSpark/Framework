using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

public sealed class PrimaryAssembly : Instance<Assembly>
{
	public static PrimaryAssembly Default { get; } = new PrimaryAssembly();

	PrimaryAssembly() : base(Start.A.Selection.Of<Assembly>()
	                              .As.Sequence.Array.By.Self.Open()
	                              .Select(x => x.SingleOrDefault(y => y.Has<HostingAttribute>()))
	                              .Ensure.Output.IsAssigned.Otherwise.Throw(PrimaryAssemblyMessage.Default)
	                              .Verified()
	                              .Get(Reflection.Assemblies.Assemblies.Default)) {}
}