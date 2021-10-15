using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

sealed class ComponentAssemblyCandidates : ISelect<AssemblyName, IEnumerable<AssemblyName>>
{
	public static ComponentAssemblyCandidates Default { get; } = new ComponentAssemblyCandidates();

	ComponentAssemblyCandidates() {}

	public IEnumerable<AssemblyName> Get(AssemblyName parameter)
	{
		var queue = new Stack<string>(parameter.Name!.Split('.'));
		while (queue.Any())
		{
			yield return new AssemblyName(string.Join(".", queue.Reverse()));
			queue.Pop();
		}
	}
}