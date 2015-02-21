using DragonSpark.Extensions;
using System;
using System.Reflection;
using System.Xaml;

namespace DragonSpark.Common.Markup
{
	public class MetadataExtension : LocationBasedExtension
	{
		readonly Type attributeType;
		readonly string expression;

		public MetadataExtension( Type attributeType, string expression )
		{
			this.attributeType = attributeType;
			this.expression = expression;
		}

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetCustomAttribute( attributeType ).Evaluate( expression );
			return result;
		}
	}
}