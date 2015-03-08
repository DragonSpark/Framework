using System;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class Resource : MarkupExtension
    {
        public string Key { get; set; }

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            var result = System.Windows.Application.Current.Resources.Transform( x => x.Contains( Key ) ? x[ Key ] : null );
            return result;
        }
    }
}