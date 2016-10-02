using System.Reflection;

namespace DragonSpark.Windows.Markup
{
	public class ClrPropertyMarkupProperty : ClrMemberMarkupProperty<PropertyInfo>
	{
		public ClrPropertyMarkupProperty( object target, PropertyInfo targetProperty ) : base( targetProperty, x => targetProperty.SetValue( target, x ), () => targetProperty.GetValue( target ) ) {}
	}
}