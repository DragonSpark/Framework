using System;
using System.Globalization;
using System.Security.Principal;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Converters
{
    public class CurrentUserConverter : BooleanConverter
    {
        public new static CurrentUserConverter Instance
        {
            get { return InstanceField; }
        }	static readonly CurrentUserConverter InstanceField = new CurrentUserConverter();

        protected override bool Resolve( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var result = ServiceLocation.With<IPrincipal, bool>( x => x.Identity == value );
            return result;
        }
    }
}