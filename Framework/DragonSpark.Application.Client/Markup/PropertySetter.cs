using System.Windows.Data;
using DragonSpark.Application.Client.Extensions;
using DragonSpark.Application.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Client.Markup
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