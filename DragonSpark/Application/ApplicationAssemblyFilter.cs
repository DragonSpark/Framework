using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application
{
	public class ApplicationAssemblyFilter : AlterationBase<IEnumerable<Assembly>>
	{
		public static ApplicationAssemblyFilter Default { get; } = new ApplicationAssemblyFilter();
		ApplicationAssemblyFilter() : this( ApplicationAssemblySpecification.Default.IsSatisfiedBy ) {}

		readonly Func<Assembly, bool> specification;

		ApplicationAssemblyFilter( Func<Assembly, bool> specification )
		{
			this.specification = specification;
		}

		public override IEnumerable<Assembly> Get( IEnumerable<Assembly> parameter ) => parameter.Where( specification );
	}
}