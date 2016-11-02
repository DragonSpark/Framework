using System.Collections.Immutable;
using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IFileElement : IFileSystemElement, IAssignableSource<ImmutableArray<byte>> {}
}