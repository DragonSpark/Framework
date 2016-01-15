using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.TypeSystem
{
	public class KnownTypeFactory : FactoryBase<System.Type, System.Type[]>
	{
		readonly Assembly[] assemblies;

		public KnownTypeFactory( [Required]Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		[Freeze]
		protected override System.Type[] CreateItem( System.Type parameter ) => assemblies
																	.SelectMany( z => z.DefinedTypes )
																	.Where( z => z.IsSubclassOf( parameter ) && parameter.Namespace != "System.Data.Entity.DynamicProxies" )
																	.AsTypes()
																	.Fixed();
	}
}