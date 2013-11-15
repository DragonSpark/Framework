using DragonSpark.Extensions;
using DragonSpark.Objects.Synchronization;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;

namespace DragonSpark.Configuration
{
	public class AssemblyVersionExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetName().Version;
			return result;
		}
	}

	public class IsDebugExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetAttribute<DebuggableAttribute>() != null;
			return result;
		}
	}

	public class IsReleaseExtension : IsDebugExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = !base.ProvideValue( serviceProvider ).To<bool>();
			return result;
		}
	}

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
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetCustomAttribute( attributeType ).EvaluateValue( expression );
			return result;
		}
	}
}