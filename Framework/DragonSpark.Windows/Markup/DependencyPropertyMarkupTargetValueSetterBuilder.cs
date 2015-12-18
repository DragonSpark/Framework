using System;
using System.Windows;

namespace DragonSpark.Windows.Markup
{
	public class DependencyPropertyMarkupTargetValueSetterBuilder : MarkupTargetValueSetterFactory<DependencyObject, DependencyProperty>
	{
		public static DependencyPropertyMarkupTargetValueSetterBuilder Instance { get; } = new DependencyPropertyMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter Create( DependencyObject targetObject, DependencyProperty targetProperty )
		{
			var result = new DependencyPropertyMarkupTargetValueSetter( targetObject, targetProperty );
			return result;
		}

		protected override Type GetPropertyType( DependencyObject target, DependencyProperty property )
		{
			return property.PropertyType;
		}
	}
}