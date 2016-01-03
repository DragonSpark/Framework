using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DragonSpark.Windows.Runtime
{
	public class DomainAssemblyProvider : AssemblyProviderBase
	{
		public static DomainAssemblyProvider Instance { get; } = new DomainAssemblyProvider();

		readonly AppDomain domain;

		public DomainAssemblyProvider() : this( AppDomain.CurrentDomain )
		{}

		public DomainAssemblyProvider( [Required]AppDomain domain )
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