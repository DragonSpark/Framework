using System.Windows.Markup;

namespace DragonSpark.Application.Presentation.Configuration
{
	[ContentProperty( "Value" )]
	public class KeyValuePair
	{
		public object Key { get; set; }
		
		public object Value { get; set; }
	}
}