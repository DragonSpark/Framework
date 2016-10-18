using DragonSpark.Sources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
		public static FileElement Empty() => Create( string.Empty );

		public static FileElement Create( string textContents, Encoding encoding ) => new FileElement( encoding.GetPreamble().Concat( encoding.GetBytes( textContents ) ).ToArray() );
		public static FileElement Create( string textContents ) => new FileElement( Defaults.DefaultEncoding.GetBytes( textContents ) );

		/// <summary>
		/// Casts a string into <see cref="FileElement"/>.
		/// </summary>
		/// <param name="s">The path of the <see cref="FileElement"/> to be created.</param>
		public static implicit operator FileElement( string s ) => Create(s);

		readonly IAssignableSource<ImmutableArray<byte>> source = new SuppliedSource<ImmutableArray<byte>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FileElement"/> class with the content of <paramref name="contents"/>.
		/// </summary>
		/// <param name="contents">The actual content.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="contents"/> is <see langword="null" />.</exception>
		public FileElement( IEnumerable<byte> contents )
		{
			source.Assign( contents );
		}

		public ImmutableArray<byte> Get() => source.Get();
		object ISource.Get() => source.Get();
		public void Assign( ImmutableArray<byte> item ) => source.Assign( item );
	}
}