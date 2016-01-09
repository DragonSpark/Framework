using System;
using DragonSpark.Activation.FactoryModel;
using System.Linq;
using System.Reflection;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace DragonSpark.TypeSystem
{
	public class ApplicationAssemblyLocator : FactoryBase<Assembly>, IApplicationAssemblyLocator
	{
		readonly Func<Assembly[]> assemblies;

		public ApplicationAssemblyLocator( [Required]Func<Assembly[]> assemblies )
		{
			this.assemblies = assemblies;
		}

		protected override Assembly CreateItem() => assemblies().SingleOrDefault( assembly => assembly.GetCustomAttribute<ApplicationAttribute>() != null );
	}
}