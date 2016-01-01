using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Modularity;
using System;
using System.Reflection;
using DragonSpark.Modularity;
using Xunit;
using ModuleInfoBuilder = DragonSpark.Windows.Modularity.ModuleInfoBuilder;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class DirectoryModuleInfoProviderTests
	{
		[Theory, MoqAutoData]
		public void Cover()
		{
			using ( new DirectoryModuleInfoProvider( ModuleInfoBuilder.Instance, new[] { "Notexists.dll" }, "." ) )
			{
				/*var assembly = Assembly.ReflectionOnlyLoad( "xunit.runner.visualstudio.testadapter, Version=2.1.0.1129, Culture=neutral, PublicKeyToken=null" );
				Assert.Null( assembly );*/
			}
		}

		[Theory, MoqAutoData]
		public void NotExist( RemoteModuleInfoProviderFactory sut )
		{
			using ( var provider = sut.Create( new LoadRemoteModuleInfoParameter( new[] { typeof( IModule ).Assembly.Location }, DirectoryModuleCatalogTests.InvalidModulesDirectory ) ) )
			{
				Assert.Empty( provider.GetModuleInfos() );
			}
		}
	}
}