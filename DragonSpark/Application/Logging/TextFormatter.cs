using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace DragonSpark.Application.Logging
{
	[ConfigurationElementType(typeof(TextFormatterData))]
	public class TextFormatter : Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.TextFormatter
	{
		public TextFormatter( string template ) : base( template, ResolveHandlers() )
		{}

		static IDictionary<string, TokenHandler<LogEntry>> ResolveHandlers()
		{
			var result = new Dictionary<string, TokenHandler<LogEntry>>
			             	{
			             		{ "formatExtendedProperty", GenericTextFormatter<LogEntry>.CreateParameterizedTokenHandler( FormatKeyValueFormatterFactory ) }
			             	};
			return result;
		}

		static Formatter<LogEntry> FormatKeyValueFormatterFactory( string parameter )
		{
			return logEntry =>
			       	{
			       		var tokens = parameter.ToStringArray( ":".ToCharArray() );
			       		object propertyObject;

			       		var result = logEntry.ExtendedProperties.TryGetValue( tokens.First(), out propertyObject )  ? string.Format( new NamedTokenFormatter(), string.Format( "{{0:{0}}}", tokens[1] ), propertyObject ) : string.Empty;
			       		return result;
			       	};
		}
	}
}