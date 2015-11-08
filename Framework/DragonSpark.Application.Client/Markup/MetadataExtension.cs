using System;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;

namespace DragonSpark.Markup
{
	public class MetadataExtension : MarkupExtension
	{
		readonly Type attributeType;
		readonly string expression;

		public MetadataExtension( Type attributeType, string expression )
		{
			this.attributeType = attributeType;
			this.expression = expression;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetCustomAttribute( attributeType ).Evaluate( expression );
			return result;
		}
	}
}