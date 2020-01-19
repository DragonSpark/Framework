using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentAssemblyNames : ISelect<AssemblyName, IEnumerable<AssemblyName>>
	{
		public static ComponentAssemblyNames Default { get; } = new ComponentAssemblyNames();

		ComponentAssemblyNames() : this(EnvironmentAssemblyName.Default) {}

		readonly Func<AssemblyName, IEnumerable<AssemblyName>> _expand;
		readonly Array<IAlteration<AssemblyName>>              _names;

		public ComponentAssemblyNames(params IAlteration<AssemblyName>[] names)
			: this(ComponentAssemblyCandidates.Default.Get, names) {}

		public ComponentAssemblyNames(Func<AssemblyName, IEnumerable<AssemblyName>> expand,
		                              Array<IAlteration<AssemblyName>> names)
		{
			_expand = expand;
			_names  = names;
		}

		public IEnumerable<AssemblyName> Get(AssemblyName parameter)
		{
			foreach (var name in _expand(parameter))
			{
				foreach (var alteration in _names.Open())
				{
					yield return alteration.Get(name);
				}
			}
		}
	}
}