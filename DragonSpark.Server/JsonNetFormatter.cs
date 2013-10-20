﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DragonSpark.Server
{
	public class IsoDateTimeConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter
	{
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteRaw( @"{ ""type"": ""System.DateTime"", ""value"":" );
			base.WriteJson( writer, value, serializer );
			writer.WriteRaw( @"}" );
		}
	}

	/*public class VersionConverter : Newtonsoft.Json.Converters.VersionConverter
	{
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteRaw( @"{ ""type"": ""System.Version"", ""value"":" );
			base.WriteJson( writer, value, serializer );
			writer.WriteRaw( @"}" );
		}
	}*/

	public class JsonNetFormatter : MediaTypeFormatter
	{
		readonly JsonSerializerSettings settings;

		public JsonNetFormatter( JsonSerializerSettings jsonSerializerSettings = null )
		{
			settings = jsonSerializerSettings ?? new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
			settings.Converters.Add( new IsoDateTimeConverter() );
			settings.Converters.Add( new Newtonsoft.Json.Converters.VersionConverter() );


			SupportedMediaTypes.Add( new MediaTypeHeaderValue( "application/json" ) );
			SupportedEncodings.Add( new UTF8Encoding( false, true ) );
		}

		public override bool CanReadType( Type type )
		{
			return true;
		}

		public override bool CanWriteType( Type type )
		{
			return true;
		}

		public override Task<object> ReadFromStreamAsync( Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger )
		{
			// Create a serializer
			var serializer = JsonSerializer.Create( settings );

			// Create task reading the content
			return Task.Factory.StartNew( () =>
			{
				using ( var streamReader = new StreamReader( readStream, SupportedEncodings.First() ) )
				{
					using ( var jsonTextReader = new JsonTextReader( streamReader ) )
					{
						return serializer.Deserialize( jsonTextReader, type );
					}
				}
			} );
		}

		public override Task WriteToStreamAsync( Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext )
		{
			// Create a serializer
			var serializer = JsonSerializer.Create( settings );

			// Create task writing the serialized content
			return Task.Factory.StartNew( () =>
			{
				using ( var jsonTextWriter = new JsonTextWriter( new StreamWriter( writeStream, SupportedEncodings.First() ) ) { CloseOutput = false } )
				{
					serializer.Serialize( jsonTextWriter, value );
					jsonTextWriter.Flush();
				}
			} );
		}
	}
}