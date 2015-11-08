// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Microsoft.Expression.Interactions.Extensions.DataHelpers {

	/// <summary>
	/// Helper for converting objects to different types.
	/// </summary>
	public static class ConverterHelper {

		/// <summary>
		/// Attempts to convert the provided value to the specified type
		/// </summary>
		/// <param name="value">object to be converted</param>
		/// <param name="type">Type to be converted to</param>
		/// <returns></returns>
		public static object ConvertToType(object value, Type type) {
			if (value == null)
				return null;

			if (type.IsAssignableFrom(value.GetType()))
				return value;

			TypeConverter converter = ConverterHelper.GetTypeConverter(type);

			if (converter != null && converter.CanConvertFrom(value.GetType())) {

				value = converter.ConvertFrom(value);
				return value;
			}

			return null;
		}

		/// <summary>
		/// Finds the type converter for the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static TypeConverter GetTypeConverter(Type type) {
			TypeConverterAttribute attribute = (TypeConverterAttribute)Attribute.GetCustomAttribute(type, typeof(TypeConverterAttribute), false);
			if (attribute != null) {
				try {
					Type converterType = Type.GetType(attribute.ConverterTypeName, false);
					if (converterType != null) {
						return (Activator.CreateInstance(converterType) as TypeConverter);
					}
				}
				catch {
				}
			}
			return new ConvertFromStringConverter(type);
		}
	}

	/// <summary>
	/// General string to object converter which uses the internal
	/// platforms type converters.
	/// </summary>
	public class ConvertFromStringConverter : TypeConverter {

		private Type type;

		/// <summary>
		/// General purpose converter that converts from a string to the specified type.
		/// </summary>
		/// <param name="type"></param>
		public ConvertFromStringConverter(Type type) {
			this.type = type;
		}

		/// <summary>
		/// Allow conversion from strings.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
			return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
		}

		/// <summary>
		/// Convert the value
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
			string stringValue = value as string;
			if (stringValue != null) {
				if (this.type == typeof(bool)) {
					return bool.Parse(stringValue);
				}
				if (this.type.IsEnum) {
					return Enum.Parse(this.type, stringValue, false);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<ContentControl xmlns='http://schemas.microsoft.com/client/2007' xmlns:c='" + ("clr-namespace:" + this.type.Namespace + ";assembly=" + this.type.Assembly.FullName.Split(new char[] { ',' })[0]) + "'>\n");
				stringBuilder.Append("<c:" + this.type.Name + ">\n");
				stringBuilder.Append(stringValue);
				stringBuilder.Append("</c:" + this.type.Name + ">\n");
				stringBuilder.Append("</ContentControl>");
#if SILVERLIGHT
				ContentControl instance = XamlReader.Load(stringBuilder.ToString()) as ContentControl;
#else
				MemoryStream memoryStream = new MemoryStream();
				StreamWriter streamWriter = new StreamWriter(memoryStream);
				streamWriter.Write(stringBuilder.ToString());
				memoryStream.Seek(0, SeekOrigin.Begin);
				ContentControl instance = XamlReader.Load(memoryStream) as ContentControl;
#endif
				if (instance != null) {
					return instance.Content;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
