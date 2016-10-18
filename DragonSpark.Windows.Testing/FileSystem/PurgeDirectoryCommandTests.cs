using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using System.Diagnostics;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class PurgeDirectoryCommandTests
	{
		[Fact]
		public void Verify()
		{
			var system = FileSystemFactory.Default.Get( @".\Testes.txt", @".\SomeDirectory" );
			PurgeDirectoryCommand.Default.Execute( system );
			Debugger.Break();
		}
	}
}