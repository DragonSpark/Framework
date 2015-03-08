using System;
using System.Reflection;

namespace DragonSpark.Runtime
{
	public partial class PropertyInfoTypeConverterValueConverter : TypeConverterValueConverter
	{
		public PropertyInfoTypeConverterValueConverter( PropertyInfo info ) : this( info, null )
		{}

		public PropertyInfoTypeConverterValueConverter( PropertyInfo info, Type typeConverterType ) : base( typeConverterType ?? ResolvePropertyConverter( info ) )
		{}

		/*public PropertyInfoTypeConverterValueConverter( PropertyDescriptor info ) : this( info, null )
		{}

		public PropertyInfoTypeConverterValueConverter( PropertyDescriptor info, Type typeConverterType ) : base(  )
		{}*/
	}
}