using System.Reflection;

namespace DragonSpark.Application.Presentation.Configuration
{
	public class PropertySetter
	{
		public string PropertyName { get; set; }

		public object Value { get; set; }

		protected internal virtual void Apply( PropertyInfo info, object target )
		{
			info.SetValue( target, Value, null );
		}
	}
}