using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IElementSource<T> : IAssignableSource<T> where T : class, IFileSystemElement
	{
		string Path { get; }
	}
}