using System.IO;

namespace DragonSpark.Serialization
{
	public static partial class XamlSerializationHelper
	{
		/*
		readonly IStreamResolver resolver;

		public XamlSerializationHelper( IStreamResolver resolver )
		{
			this.resolver = resolver;
		}
		*/

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", MessageId = "T", Justification = "The Generic type parameter is used to specify the return type." )]
		public static TResult Load<TResult>( Stream stream )
		{
			if ( stream != null )
			{
				using ( stream )
				{
					var result = ResolveLoad<TResult>( stream );
					return result;
				}
			}
			return default(TResult);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", MessageId = "T", Justification = "The Generic type parameter is used to specify the return type." )]
		public static TResult Load<TResult>( IStreamResolver resolver )
		{
			var stream = resolver.ResolveStream();
			var result = Load<TResult>( stream );
			return result;
		}

		public static object Load( Stream stream )
		{
			var result = Load<object>( stream );
			return result;
		}

		public static object Load( IStreamResolver resolver )
		{
			var result = Load<object>( resolver );
			return result;
		}

		/*public TResultType Create<TResultType>( Type type, string name )
		{
			var result = Create<TResultType>( type.Assembly, name );
			return result;
		}

		public TResultType Create<TResultType>( Assembly assembly, string name )
		{
			using ( var stream = assembly.GetManifestResourceStream( name ) )
			{
				var result = stream != null ? (TResultType)XamlReader.Load( stream ) : default(TResultType);
				return result;
			}
		}

		public TResultType Create<TResultType>( string filePath )
		{
			if ( File.Exists( filePath ) )
			{
				var xaml = File.ReadAllText( filePath );
				var result = CreateFromXaml<TResultType>( xaml );
				return result;
			}
			return default(TResultType);
		}

		public TResultType CreateFromXaml<TResultType>( string xaml )
		{
			var result = (TResultType)XamlReader.Parse( xaml );
			return result;
		}*/
	}
}
