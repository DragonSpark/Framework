using System;
using System.Reflection;

namespace DragonSpark.Windows.Markup
{
	public class FieldInfoMarkupTargetValueSetterBuilder : MarkupTargetValueSetterFactory<FieldInfo>
	{
		public static FieldInfoMarkupTargetValueSetterBuilder Instance { get; } = new FieldInfoMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter Create( object targetObject, FieldInfo targetProperty )
		{
			return new ClrMemberMarkupTargetValueSetter<FieldInfo>( targetObject, targetProperty, ( o, info, value ) => info.SetValue( o, value ) );
		}

		protected override Type GetPropertyType( object target, FieldInfo property )
		{
			return property.FieldType;
		}
	}
}