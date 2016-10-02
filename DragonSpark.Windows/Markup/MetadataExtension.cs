using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Reflection;
using System.Xaml;

namespace DragonSpark.Windows.Markup
{
	public class MetadataExtension : MarkupExtensionBase
	{
		readonly Type attributeType;
		readonly string expression;

		public MetadataExtension( Type attributeType, string expression )
		{
			this.attributeType = attributeType;
			this.expression = expression;
		}

		[Service]
		public IExpressionEvaluator Evaluator { [return: NotNull]get; set; }

		protected override object GetValue( MarkupServiceProvider serviceProvider ) 
			=> Evaluator.Evaluate( serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetCustomAttribute( attributeType ), expression );
	}
}