using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Assemblies;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class AssemblySelectorQuery : ISelect<Assembly[], Assembly[]>
	{
		readonly Func<Assembly, AssemblyName>                  _name;
		readonly Func<AssemblyName, IEnumerable<AssemblyName>> _names;
		readonly Func<AssemblyName, Assembly>                  _load;

		public AssemblySelectorQuery(Func<AssemblyName, IEnumerable<AssemblyName>> names)
			: this(AssemblyNameSelector.Default.Get, names, Load.Default.Get) {}

		public AssemblySelectorQuery(Func<Assembly, AssemblyName> name,
		                             Func<AssemblyName, IEnumerable<AssemblyName>> names,
		                             Func<AssemblyName, Assembly> load)
		{
			_name  = name;
			_names = names;
			_load  = load;
		}

		public Assembly[] Get(Assembly[] parameter) => parameter.Select(_name)
		                                                        .SelectMany(_names)
		                                                        .Select(_load)
		                                                        .AsValueEnumerable()
		                                                        .Where(y => y != null)
		                                                        .Distinct()
		                                                        .ToArray();
	}
}