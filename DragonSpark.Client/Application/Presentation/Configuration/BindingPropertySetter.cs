using System.Windows.Data;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
	public class BindingPropertySetter : PropertySetter
	{
		protected internal override void Apply( System.Reflection.PropertyInfo info, object target )
		{
			Value.As<Binding>( x => x.ApplyTo( target, y =>
			{
				Value = y;
				base.Apply( info, target );
			} ) ).Null( () => base.Apply( info, target ) );
		}
	}
}