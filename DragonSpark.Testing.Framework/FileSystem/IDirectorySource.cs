using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IDirectorySource : IAssignableSource<string>
	{
		string PathRoot { get; }
	}
}