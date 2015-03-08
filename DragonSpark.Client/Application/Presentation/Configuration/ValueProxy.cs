using System.Windows.Markup;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
	[ContentProperty( "Value" )]
	public class ValueProxy : ViewObject
	{
		public object Value
		{
			get { return _value; }
			set
			{
				if ( _value != value )
				{
					_value = value;
					Environment.IsInDesignMode.IsFalse( () => NotifyOfPropertyChange( () => Value ) );
				}
			}
		}	object _value;
	}
}