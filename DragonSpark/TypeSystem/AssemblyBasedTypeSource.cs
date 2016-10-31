using DragonSpark.Application;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class AssemblyBasedTypeSource : SuppliedTypeSource
	{
		readonly static Func<Assembly, IEnumerable<Type>> All = AssemblyTypes.All.GetEnumerable;

		public AssemblyBasedTypeSource( params Type[] types ) : this( types, Items<Assembly>.Default ) {}

		public AssemblyBasedTypeSource( IEnumerable<Type> types, params Assembly[] assemblies ) : this( types.Assemblies().Union( assemblies ) ) {}

		public AssemblyBasedTypeSource( IEnumerable<Assembly> assemblies ) : base( assemblies.SelectMany( All ) ) {}
	}
}