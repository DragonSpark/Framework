using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Reflection.Assemblies;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class AssemblySelectorQuery : IMaterialize<Assembly>
	{
		readonly Func<AssemblyName, Assembly>                  _load;
		readonly Func<Assembly, AssemblyName>                  _name;
		readonly Func<AssemblyName, IEnumerable<AssemblyName>> _names;

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
		                                                        .Where(y => y.Account() is not null)
		                                                        .Distinct()
		                                                        .ToArray();
	}
}