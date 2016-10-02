using System.Collections.Immutable;

namespace DragonSpark.Application
{
	public interface IExportProvider
	{
		ImmutableArray<T> GetExports<T>( string name = null );
	}
}