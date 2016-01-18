using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Windows.Markup
{
	[ContentProperty( nameof(Properties) )]
	public class LocateExtension : MonitoredMarkupExtension
	{
		public LocateExtension()
		{}

		public LocateExtension( Type type, string buildName = null )
		{
			Type = type;
			BuildName = buildName;
		}

		[Required]
		public Type Type { get; set; }

		public string BuildName { get; set; }

		[Locate, Required]
		IServiceLocator Locator { [return: NotNull]get; set; }

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var result = Type.With( x => Locator.GetInstance( x, BuildName ) );
			result.As<ISupportInitialize>( x => x.BeginInit() );
			result.With( x => Properties.Each( y => x.GetType().GetProperty( y.PropertyName ).With( z => y.Apply( z, x ) ) ) );
			result.As<ISupportInitialize>( x => x.EndInit() );
			return result;
		}

		public Collection<PropertySetter> Properties { get; } = new Collection<PropertySetter>();
	}
}