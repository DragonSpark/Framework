using DragonSpark.Setup.Registration;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows.Entity
{
	[Register]
	public class DataDirectoryPath : AppDomainValue<string>
	{
		public const string Key = "DataDirectory";

		public DataDirectoryPath() : base( Key )
		{}
	}
}