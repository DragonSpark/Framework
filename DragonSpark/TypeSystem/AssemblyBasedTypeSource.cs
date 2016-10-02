using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class AssemblyBasedTypeSource : SuppliedTypeSource
	{
		readonly static Func<Assembly, IEnumerable<Type>> All = AssemblyTypes.All.ToDelegate();

		public AssemblyBasedTypeSource( params Type[] types ) : this( types, Items<Assembly>.Default ) {}

		public AssemblyBasedTypeSource( IEnumerable<Type> types, params Assembly[] assemblies ) : this( types.Assemblies().Union( assemblies ) ) {}

		public AssemblyBasedTypeSource( IEnumerable<Assembly> assemblies ) : base( assemblies.SelectMany( All ) ) {}
	}
}