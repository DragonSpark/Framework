using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application
{
	public class RegisteredAssemblyFilter : AlterationBase<IEnumerable<Assembly>>
	{
		public static RegisteredAssemblyFilter Default { get; } = new RegisteredAssemblyFilter();
		RegisteredAssemblyFilter() : this( RegisteredAssemblySpecification.Default.IsSatisfiedBy ) {}

		readonly Func<Assembly, bool> specification;

		RegisteredAssemblyFilter( Func<Assembly, bool> specification )
		{
			this.specification = specification;
		}

		public override IEnumerable<Assembly> Get( IEnumerable<Assembly> parameter ) => parameter.Where( specification );
	}
}