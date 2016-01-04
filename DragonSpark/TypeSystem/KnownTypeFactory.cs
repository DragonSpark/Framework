using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;

namespace DragonSpark.TypeSystem
{
	public class KnownTypeFactory : FactoryBase<Type, Type[]>
	{
		readonly Assembly[] assemblies;

		public KnownTypeFactory( Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		[Cache]
		protected override Type[] CreateItem( Type parameter )
		{
			var result = assemblies.SelectMany( z => z.DefinedTypes ).Where( z => z.IsSubclassOf( parameter ) && parameter.Namespace != "System.Data.Entity.DynamicProxies" ).AsTypes().Fixed();
			return result;
		}
	}
}