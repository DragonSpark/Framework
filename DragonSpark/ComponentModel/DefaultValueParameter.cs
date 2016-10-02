using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public struct DefaultValueParameter
	{
		public DefaultValueParameter( object instance, PropertyInfo metadata )
		{
			Instance = instance;
			Metadata = metadata;
		}

		public object Instance { get; }

		public PropertyInfo Metadata { get; }
	}
}