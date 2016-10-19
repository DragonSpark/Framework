using DragonSpark.Sources;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IElementSource<T> : IAssignableSource<T> where T : class, IFileSystemElement
	{
		string Path { get; }
	}

	sealed class ElementSource<T> : SuppliedSource<T>, IElementSource<T> where T : class, IFileSystemElement
	{
		readonly IFileSystemRepository repository;
		readonly string filePath;

		public ElementSource( IFileSystemRepository repository, string filePath )
		{
			this.repository = repository;
			this.filePath = filePath;
		}

		public string Path => base.Get()?.Path ?? filePath;

		public override T Get() => base.Get() ?? Locate();

		T Locate()
		{
			var element = repository.GetElement( filePath );
			if ( element == null )
			{
				throw new FileNotFoundException( "Element not found at specified path.", filePath );
			}
			var result = (T)element;
			Assign( result );
			return result;
		}
	}
}