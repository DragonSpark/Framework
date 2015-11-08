using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xaml;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace DragonSpark.Windows
{
	public static class Process
	{
		static readonly IList<System.Diagnostics.Process> Source = new List<System.Diagnostics.Process>();
		public static IEnumerable<System.Diagnostics.Process> Processes { get; } = new ReadOnlyCollection<System.Diagnostics.Process>( Source );

		[SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Wrapper around native method to internally track processes created by custom application." )]
		public static System.Diagnostics.Process Create( string file )
		{
			var target = System.Diagnostics.Process.Start( file );
			target.NotNull( x =>
			{
				x.Exited += ResultExited;
				Source.Add( x );
			} );
			return target;
		}

		[SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Convenience method." )]
		public static bool Exit( System.Diagnostics.Process process )
		{
			process.Kill();
			process.WaitForExit();
			return process.HasExited;
		}

		static void ResultExited( object sender, EventArgs e )
		{
			Source.Remove( sender.As<System.Diagnostics.Process>() );
		}
	}

	public interface IDataTransformer
	{
		object Transform( IXPathNavigable stylesheet, IXPathNavigable source );

		string ToString( IXPathNavigable stylesheet, IXPathNavigable source );
	}

	public class DataTransformer : IDataTransformer
	{
		public object Transform( IXPathNavigable stylesheet, IXPathNavigable source )
		{
			var transform = new XslCompiledTransform();
			transform.Load( stylesheet );

			var stream = new MemoryStream();
			transform.Transform( source, null, stream );
			stream.Seek( 0, SeekOrigin.Begin );

			var result = XamlServices.Load( stream );
			return result;
		}

		[SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope" )]
		public string ToString( IXPathNavigable stylesheet, IXPathNavigable source )
		{
			var transform = new XslCompiledTransform();
			transform.Load( stylesheet );

			var stream = new MemoryStream();
			transform.Transform( source, null, stream );
			stream.Seek( 0, SeekOrigin.Begin );

			var result = new StreamReader( stream ).ReadToEnd();
			return result;
		}
	}

	public static class DataBuilder
	{
		public static IXPathNavigable Create( string data )
		{
			var result = new XmlDocument();
			result.LoadXml( data );
			return result;
		}

		public static IXPathNavigable Create( Uri location )
		{
			var result = new XmlDocument();
			result.Load( location.ToString() );
			return result;
		}
	}

	public static class DataTransformerExtensions
	{
		public static TResult Transform<TResult>( this IDataTransformer target, string stylesheetXml, string sourceXml ) where TResult : class
		{
			var stylesheet = DataBuilder.Create( stylesheetXml );
			var source = DataBuilder.Create( sourceXml );

			var result = target.Transform<TResult>( stylesheet, source );
			return result;
		}

		public static TResult Transform<TResult>( this IDataTransformer target, IXPathNavigable stylesheet, IXPathNavigable source ) where TResult : class
		{
			var result = target.Transform( stylesheet, source ).To<TResult>();
			return result;
		}
	}
}