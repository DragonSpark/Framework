using System;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class ApplicationService : MarkupExtension
	{
		public string TypeName { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var type = serviceProvider.GetService( typeof(IXamlTypeResolver) ).As<IXamlTypeResolver>().Transform( x => x.Resolve( TypeName ) );
			var result = type.Transform( x => System.Windows.Application.Current.ApplicationLifetimeObjects.Cast<object>().FirstOrDefault( x.IsInstanceOfType ) );
			return result;
		}
	}
}