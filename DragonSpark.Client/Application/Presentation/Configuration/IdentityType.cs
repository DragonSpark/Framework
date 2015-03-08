using System;
using System.Security.Principal;
using System.Windows.Markup;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class IdentityType : MarkupExtension
    {
        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            var result = ServiceLocation.With<IPrincipal,System.Type>( x => x.Identity.GetType() );
            return result;
        }
    }
}