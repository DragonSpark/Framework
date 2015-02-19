using System.Windows.Controls;
using System.Windows.Markup;

namespace DragonSpark.Client.Windows.Markup
{
	public class ConfigurationObject : Control, IAddChild
	{
		void IAddChild.AddChild( object value )
		{}

		void IAddChild.AddText( string text )
		{}
	}
}