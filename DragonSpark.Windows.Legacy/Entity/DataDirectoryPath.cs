using DragonSpark.Windows.Runtime;
using JetBrains.Annotations;
using System.Composition;

namespace DragonSpark.Windows.Legacy.Entity
{
	[Export, Shared, UsedImplicitly]
	public sealed class DataDirectoryPath : AppDomainStore<string>
	{
		[UsedImplicitly]
		public const string Key = "DataDirectory";

		public DataDirectoryPath() : base( Key ) {}
	}
}