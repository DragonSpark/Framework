using System;
using System.Linq;
using DragonSpark.Modularity;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	/// <summary>
	/// Summary description for ModuleInfoGroupExtensionsTests
	/// </summary>
	public class ModuleInfoGroupExtensionsTests
	{
		[Fact]
		public void ShouldAddModuleToModuleInfoGroup()
		{
			string moduleName = "MockModule";
			ModuleInfoGroup groupInfo = new ModuleInfoGroup();
			groupInfo.AddModule(moduleName, typeof(MockModule));

			Assert.Equal(1, groupInfo.Count);
			Assert.Equal<string>(moduleName, groupInfo.ElementAt(0).ModuleName);
		}

		[Fact]
		public void ShouldSetModuleTypeCorrectly()
		{
			ModuleInfoGroup groupInfo = new ModuleInfoGroup();
			groupInfo.AddModule("MockModule", typeof(MockModule));

			Assert.Equal<int>(1, groupInfo.Count);
			Assert.Equal<string>(typeof(MockModule).AssemblyQualifiedName, groupInfo.ElementAt(0).ModuleType);
		}

		[Fact]
		public void NullTypeThrows()
		{
			var groupInfo = new ModuleInfoGroup();
			Assert.Throws<ArgumentNullException>( () => groupInfo.AddModule("NullModule", null) );
		}

		[Fact]
		public void ShouldSetDependencies()
		{
			string dependency1 = "ModuleA";
			string dependency2 = "ModuleB";

			ModuleInfoGroup groupInfo = new ModuleInfoGroup();
			groupInfo.AddModule("MockModule", typeof(MockModule), dependency1, dependency2);

			var dependsOn = groupInfo.ElementAt(0).DependsOn;
			Assert.NotNull(dependsOn);
			Assert.Equal(2, dependsOn.Count);
			Assert.True(dependsOn.Contains(dependency1));
			Assert.True(dependsOn.Contains(dependency2));
		}

		[Fact]
		public void ShouldUseTypeNameIfNoNameSpecified()
		{
			ModuleInfoGroup groupInfo = new ModuleInfoGroup();
			groupInfo.AddModule(typeof(MockModule));

			Assert.Equal<int>(1, groupInfo.Count);
			Assert.Equal<string>(typeof(MockModule).Name, groupInfo.ElementAt(0).ModuleName);
		}
		
		class MockModule : IModule
		{
			public void Initialize()
			{}

			public void Load()
			{
			}
		}
	}
}
