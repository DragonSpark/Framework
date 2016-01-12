using System;
using System.Collections.Generic;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.TypeSystem
{
	public static class Assemblies
	{
		public static Assembly[] GetCurrent() => Services.Locate<Assembly[]>() ?? Default<Assembly>.Items;

		public delegate Assembly[] Get();

		public static IEnumerable<Assembly> Or( this Get @this, Func<Assembly> or )
		{
			var result = @this().AnyOr( () => or().Append( typeof(Assemblies).Assembly() ).Distinct() );
			return result;
		}
	}

	public class ApplicationAssemblyLocator : FactoryBase<Assembly>, IApplicationAssemblyLocator
	{
		readonly Assembly[] assemblies;

		public ApplicationAssemblyLocator( [Required]Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		protected override Assembly CreateItem() => assemblies.SingleOrDefault( assembly => assembly.GetCustomAttribute<ApplicationAttribute>() != null );
	}
}