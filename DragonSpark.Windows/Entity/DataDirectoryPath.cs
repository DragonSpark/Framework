using DragonSpark.Windows.Runtime;
using System.Composition;

namespace DragonSpark.Windows.Entity
{
	[Export, Shared]
	public class DataDirectoryPath : AppDomainStore<string>
	{
		public const string Key = "DataDirectory";

		public DataDirectoryPath() : base( Key ) {}
	}
}