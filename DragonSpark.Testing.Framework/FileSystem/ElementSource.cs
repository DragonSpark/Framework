using DragonSpark.Sources;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	sealed class ElementSource<T> : SuppliedSource<T>, IElementSource<T> where T : class, IFileSystemElement
	{
		readonly IFileSystemRepository repository;
		
		public ElementSource( IFileSystemRepository repository, string filePath )
		{
			this.repository = repository;
			Path = filePath;
		}

		public string Path { get; private set; }

		protected override void OnAssign( T item = default(T) )
		{
			Path = item != null ? repository.GetPath( item ) : Path;
			base.OnAssign( item );
		}

		public override T Get() => base.Get() ?? Locate();

		T Locate()
		{
			var element = repository.Get( Path );
			if ( element == null )
			{
				throw new FileNotFoundException( "Element not found at specified path.", Path );
			}
			var result = (T)element;
			Assign( result );
			return result;
		}
	}
}