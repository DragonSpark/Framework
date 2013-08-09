using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using DragonSpark.Extensions;

namespace DragonSpark.IoC
{
	public class NamedTypeBuildKeyConverter : TypeConverter
	{
		const string BuildKeyRegEx = @"Build Key\[(.*), (.*?)\]";

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			string input = value as string;
			if ( input != null )
			{
				return ResolveKey( input );
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if ( destinationType == typeof(string) && value is NamedTypeBuildKey )
			{
				NamedTypeBuildKey key = (NamedTypeBuildKey)value;
				return key.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		static NamedTypeBuildKey ResolveKey( string input )
		{
			if ( Regex.IsMatch( input, BuildKeyRegEx, RegexOptions.Singleline ) )
			{
				var match = Regex.Match( input, BuildKeyRegEx, RegexOptions.Singleline );
				string typeName = match.Groups[1].Value,
				       name = match.Groups[2].Value;
				// UnityTypeResolver resolver = ThreadLocalStorage.Peek<UnityTypeResolver>();
				var converter = new AssemblyQualifiedTypeNameConverter();
				var type = converter.ConvertFrom( typeName ).To<Type>();
				var result = new NamedTypeBuildKey( type, name == "null" ? null : name );
				return result;
			}
			return default(NamedTypeBuildKey);
		}
	}
}