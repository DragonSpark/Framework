using System;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
    public class InstanceTypeConverter : ValueConverterBase<object,Type>
    {
        public static InstanceTypeConverter Instance
        {
            get { return InstanceField; }
        }	static readonly InstanceTypeConverter InstanceField = new InstanceTypeConverter();

        protected override object PerformConversion( object value, Type parameter )
        {
            var result = value.Transform( x => x.GetType() );
            return result;
        }
    }
}