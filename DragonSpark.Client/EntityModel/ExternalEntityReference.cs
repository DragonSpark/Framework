using System.Collections.Generic;
using System.Reflection;

namespace Common.EntityModel
{
	public class ExternalEntityReference
	{
		readonly PropertyInfo entityProperty;
		readonly IDictionary<string, PropertyInfo> keys;

		public ExternalEntityReference( PropertyInfo entityProperty, IDictionary<string,PropertyInfo> keys )
		{
			this.entityProperty = entityProperty;
			this.keys = keys;
		}

		public IDictionary<string, PropertyInfo> Keys
		{
			get { return keys; }
		}

		public PropertyInfo EntityProperty
		{
			get { return entityProperty; }
		}
	}
}