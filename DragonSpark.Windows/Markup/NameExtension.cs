using System;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	[MarkupExtensionReturnType( typeof(string) )]
	public class NameExtension : MarkupExtension
	{
		public NameExtension() : this( null )
		{}

		public NameExtension( Type type )
		{
			Type = type;
		}

		public Type Type { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = Type.With( x => x.Name );
			return result;
		}
	}
}