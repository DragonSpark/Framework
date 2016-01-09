using System;
using System.Reflection;
using System.Xaml;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public class MetadataExtension : MonitoredMarkupExtension
	{
		readonly Type attributeType;
		readonly string expression;

		public MetadataExtension( Type attributeType, string expression )
		{
			this.attributeType = attributeType;
			this.expression = expression;
		}

		[Activate]
		IExpressionEvaluator Evaluator { get; set; }

		[BuildUp]
		protected override object GetValue( IServiceProvider serviceProvider ) => Evaluator.Evaluate( serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetCustomAttribute( attributeType ), expression );
	}
}