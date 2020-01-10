using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	public sealed class PrimaryAssembly : Instance<Assembly>
	{
		public static PrimaryAssembly Default { get; } = new PrimaryAssembly();

		PrimaryAssembly() : base(Start.A.Selection.Of<Assembly>()
		                              .As.Sequence.Array.By.Self.Query()
		                              .Only(x => x.Has<HostingAttribute>())
		                              .Ensure.Output.IsAssigned.Otherwise.Throw(PrimaryAssemblyMessage.Default)
		                              .Return()
		                              .Get(Reflection.Assemblies.Assemblies.Default)) {}
	}
}