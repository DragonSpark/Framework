using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class Assemblies : ArrayStore<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : this(AssemblySelector.Default) {}

		public Assemblies(ISelect<Array<Assembly>, Array<Assembly>> select) : this(select, DefaultAssemblies.Default) {}

		public Assemblies(ISelect<Array<Assembly>, Array<Assembly>> select, IArray<Assembly> parameter)
			: base(select.Then().Subject.Bind(parameter).Select(x => x.Open())) {}
	}
}