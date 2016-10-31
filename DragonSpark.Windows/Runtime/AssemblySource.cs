using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DragonSpark.Windows.Runtime
{
	public sealed class AssemblySource : ItemSourceBase<Assembly>
	{
		readonly AppDomain domain;
		readonly Func<Assembly, bool> specification;
		public AssemblySource( AppDomain domain ) : this( domain, Specification.Implementation.IsSatisfiedBy ) {}
		public AssemblySource( AppDomain domain, Func<Assembly, bool> specification )
		{
			this.domain = domain;
			this.specification = specification;
		}

		protected override IEnumerable<Assembly> Yield() => domain.GetAssemblies().Where( specification );

		sealed class Specification : SpecificationBase<Assembly>
		{
			const string SystemReflectionEmitInternalAssemblyBuilder = "System.Reflection.Emit.InternalAssemblyBuilder";
			public static Specification Implementation { get; } = new Specification();
			Specification() {}

			public override bool IsSatisfiedBy( Assembly parameter ) => 
				parameter.Not<AssemblyBuilder>()
				&& parameter.GetType().FullName != SystemReflectionEmitInternalAssemblyBuilder
				&& !string.IsNullOrEmpty( parameter.Location );
		}
	}
}