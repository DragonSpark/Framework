using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.TypeSystem
{
	public class KnownTypeFactory : FactoryBase<Type, Type[]>
	{
		readonly Func<Assembly[]> assemblies;

		public KnownTypeFactory( [Required]Func<Assembly[]> assemblies )
		{
			this.assemblies = assemblies;
		}

		[Freeze]
		protected override Type[] CreateItem( Type parameter ) => assemblies()
																	.SelectMany( z => z.DefinedTypes )
																	.Where( z => z.IsSubclassOf( parameter ) && parameter.Namespace != "System.Data.Entity.DynamicProxies" )
																	.AsTypes()
																	.Fixed();
	}
}