using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Synchronization
{
	public class ReflectedPropertyMappingExpressionResolver : ISynchronizationExpressionResolver
	{
		public IEnumerable<ISynchronizationContext> Resolve( SynchronizationKey key )
		{
			var items = from type in new[] { key.First, key.Second }
			             from property in type.GetProperties( DragonSparkBindingOptions.AllProperties )
			             let normal = type == key.First
			             let attribute = SynchronizationAttribute( property )
			             where attribute != null && Check( attribute, normal, key )
			             select new SynchronizationContext( normal ? property.Name : attribute.Expression,
			                                                normal ? attribute.Expression : property.Name, attribute.TypeConverterType, false );
			var result = items.Cast<ISynchronizationContext>().ToArray();
			return result;
		}

	    static SynchronizationAttribute SynchronizationAttribute( PropertyInfo property )
		{
			var synchronizationAttribute = property.GetAttribute<SynchronizationAttribute>();
			return synchronizationAttribute;
		}

	    static bool Check( SynchronizationAttribute attribute, bool normal, SynchronizationKey key )
		{
			var isAssignableFrom = attribute.TargetType.IsAssignableFrom( normal ? key.Second : key.First );
			return isAssignableFrom;
		}
	}
}