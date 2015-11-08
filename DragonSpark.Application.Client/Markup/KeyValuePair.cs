using System.Windows.Markup;

namespace DragonSpark.Markup
{
	[ContentProperty( "Value" )]
	public class KeyValuePair
	{
		public object Key { get; set; }
		
		public object Value { get; set; }
	}
}