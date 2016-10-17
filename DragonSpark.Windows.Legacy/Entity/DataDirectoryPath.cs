using DragonSpark.Windows.Runtime;
using System.Composition;

namespace DragonSpark.Windows.Legacy.Entity
{
	[Export, Shared]
	public class DataDirectoryPath : AppDomainStore<string>
	{
		public const string Key = "DataDirectory";

		public DataDirectoryPath() : base( Key ) {}
	}
}