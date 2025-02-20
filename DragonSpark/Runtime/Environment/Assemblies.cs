using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment;

sealed class Assemblies : ArrayStore<Assembly>
{
	public Assemblies(ISelect<Array<Assembly>, Array<Assembly>> select) : this(select, DefaultAssemblies.Default) {}

	public Assemblies(ISelect<Array<Assembly>, Array<Assembly>> select, IArray<Assembly> parameter)
		: base(select.Then().Subject.Bind(parameter).Select(x => x.Open())) {}
}