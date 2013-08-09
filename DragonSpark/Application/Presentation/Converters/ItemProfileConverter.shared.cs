using System;
using System.Globalization;
using System.Windows.Data;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Converters
{
    public class ItemProfileConverter : IValueConverter
    {
        public static ItemProfileConverter Instance
        {
            get { return InstanceField; }
        }	static readonly ItemProfileConverter InstanceField = new ItemProfileConverter();

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var result = value.AsTo<IItemProfile,object>( x => ItemProfileExtensions.Activated<object>( x ) );
            return result;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}