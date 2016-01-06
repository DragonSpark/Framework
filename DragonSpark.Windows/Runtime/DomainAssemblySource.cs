using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DragonSpark.Windows.Runtime
{
	public class DomainAssemblySource : AssemblySourceBase
	{
		public static DomainAssemblySource Instance { get; } = new DomainAssemblySource();

		readonly AppDomain domain;

		public DomainAssemblySource() : this( AppDomain.CurrentDomain )
		{}

		public DomainAssemblySource( [Required]AppDomain domain )
		{
			this.domain = domain;
		}
		
		protected override Assembly[] CreateItem()
		{
			var query = from assembly in domain.GetAssemblies()
						where assembly.Not<AssemblyBuilder>()
								&& assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder"
								&& !string.IsNullOrEmpty( assembly.Location )
				orderby assembly.GetName().Name
				select assembly;
			var result = query.ToArray();
			return result;
		}
	}
}