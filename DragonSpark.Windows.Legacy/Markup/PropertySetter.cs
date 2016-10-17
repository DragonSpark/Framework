using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Windows.Legacy.Markup
{
	[ContentProperty( "Value" )]
	public class PropertySetter
	{
		public string PropertyName { get; set; }

		public object Value { get; set; }

		internal void Apply( PropertyInfo info, object target )
		{
			OnApply( info, target );
		}

		protected virtual void OnApply( PropertyInfo info, object target )
		{
			info.SetValue( target, Value, null );
		}
	}
}