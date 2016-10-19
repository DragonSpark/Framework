using DragonSpark.Sources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// The class represents the associated data of a file.
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class FileElement : FileSystemElementBase, IFileElement
	{
		public static FileElement Empty( string path ) => Create( path, string.Empty );

		public static FileElement Create( string path, [Optional]string textContents, Encoding encoding ) => new FileElement( path, encoding.GetPreamble().Concat( encoding.GetBytes( textContents ?? string.Empty ) ).ToArray() );
		public static FileElement Create( string path, [Optional]string textContents ) => new FileElement( path, Defaults.DefaultEncoding.GetBytes( textContents ?? string.Empty ) );

		readonly IAssignableSource<ImmutableArray<byte>> source = new SuppliedSource<ImmutableArray<byte>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FileElement"/> class with the content of <paramref name="contents"/>.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="contents">The actual content.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="contents"/> is <see langword="null" />.</exception>
		public FileElement( string path, IEnumerable<byte> contents ) : base( path )
		{
			source.Assign( contents );
		}

		public ImmutableArray<byte> Get() => source.Get();
		object ISource.Get() => source.Get();
		public void Assign( ImmutableArray<byte> item ) => source.Assign( item );
	}
}