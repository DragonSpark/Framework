using DragonSpark.Client.Windows.Extensions;
using DragonSpark.Common.Markup;
using DragonSpark.Extensions;
using System.Windows.Data;

namespace DragonSpark.Client.Windows.Markup
{
	public class BindingPropertySetter : PropertySetter
	{
		protected override void OnApply( System.Reflection.PropertyInfo info, object target )
		{
			Value.As<Binding>( x => x.ApplyTo( target.DetermineFrameworkElement(), y =>
			{
				Value = y;
				base.OnApply( info, target );
			} ) ).Null( () => base.OnApply( info, target ) );
		}
	}
}