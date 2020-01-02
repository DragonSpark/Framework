using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Assemblies;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class Assemblies : ArrayResult<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : this(PrimaryAssembly.Default, HostingAssembly.Default) {}

		public Assemblies(params Assembly[] input) : base(Start.A.Selection.Of.Type<Assembly>()
		                                                       .As.Sequence.Array.By.Self.Query()
		                                                       .Select(AssemblyNameSelector.Default)
		                                                       .SelectMany(ComponentAssemblyNames.Default)
		                                                       .Select(Load.Default)
		                                                       .Append(Sequence.From(input))
		                                                       .WhereBy(y => y != null)
		                                                       .Distinct()
		                                                       .Get()
		                                                       .In(input.Result)
		                                                       .Get) {}
	}
}