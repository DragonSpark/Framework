using System;
using System.Reflection;

namespace DragonSpark.Windows.Markup
{
	public class PropertyInfoMarkupTargetValueSetterBuilder : MarkupTargetValueSetterFactory<PropertyInfo>
	{
		public static PropertyInfoMarkupTargetValueSetterBuilder Instance { get; } = new PropertyInfoMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter Create( object targetObject, PropertyInfo targetProperty )
		{
			return new ClrMemberMarkupTargetValueSetter<PropertyInfo>( targetObject, targetProperty, ( o, info, value ) => info.SetValue( o, value ) );
		}

		protected override Type GetPropertyType( object target, PropertyInfo property )
		{
			return property.PropertyType;
		}
	}
}