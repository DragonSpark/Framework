using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Presentation.View
{
	public class AvailabilityEffectConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return typeof(string).IsAssignableFrom(sourceType) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		/// The conversion cannot be performed.
		/// </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var theString = value as string;

			switch (theString)
			{
				case "None":
					return AvailabilityEffect.None;
				case "Disable":
					return AvailabilityEffect.Disable;
				case "Hide":
					return AvailabilityEffect.Hide;
				case "Collapse":
					return AvailabilityEffect.Collapse;
				default:
					return Environment.IsInDesignMode || string.IsNullOrEmpty(theString) ? AvailabilityEffect.None : ServiceLocator.Current.GetInstance(typeof(IAvailabilityEffect), theString);
			}
		}
	}
}