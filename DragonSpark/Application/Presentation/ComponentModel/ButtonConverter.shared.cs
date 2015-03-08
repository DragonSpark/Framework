using System;
using System.ComponentModel;
using System.Globalization;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	public class ButtonConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if(typeof(string).IsAssignableFrom(sourceType))
				return true;
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var theString = value.ToString();
			var parts = theString.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

			var collection = new ViewCollection<ButtonModel>();
			parts.Apply(x => collection.Add(new ButtonModel(x)));

			return collection;
		}
	}
}