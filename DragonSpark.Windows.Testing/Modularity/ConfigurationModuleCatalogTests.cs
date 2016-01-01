using System;
using System.Configuration;
using System.Linq;
using DragonSpark.Windows.Modularity;
using DragonSpark.Windows.Testing.TestObjects.Modules;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class ConfigurationModuleCatalogTests
	{
		[Fact]
		public void CanInitConfigModuleEnumerator()
		{
			var catalog = new ConfigurationModuleCatalog { Store = new MockConfigurationStore() };
			Assert.NotNull(catalog);
		}

		[Fact]
		public void NullConfigurationStoreThrows()
		{
			var catalog = new ConfigurationModuleCatalog { Store = null };
			Assert.Throws<InvalidOperationException>( new Action( catalog.Load ) );
			
		}

		[Fact]
		public void ShouldReturnAListOfModuleInfo()
		{
			var store = new MockConfigurationStore
			{
				Modules = new[] { new ModuleConfigurationElement( @"MocksModules\MockModuleA.dll", "TestModules.MockModuleAClass", "MockModuleA", false ) }
			};

			var catalog = new ConfigurationModuleCatalog { Store = store };
			catalog.Load();

			var modules = catalog.Modules;
			Assert.NotNull( modules );
			Assert.Equal( 1, modules.Count() );
			var first = modules.OfType<DynamicModuleInfo>().First();
			Assert.NotEqual( InitializationMode.WhenAvailable, first.InitializationMode );
			Assert.NotNull( first.Ref );
			Assert.StartsWith( Uri.UriSchemeFile, first.Ref );
			Assert.True( first.Ref.Contains( @"MocksModules/MockModuleA.dll" ) );
			Assert.NotNull( first.ModuleType );
			Assert.Equal( "TestModules.MockModuleAClass", first.ModuleType );

		}

		[Fact]
		public void GetZeroModules()
		{
			MockConfigurationStore store = new MockConfigurationStore();
			ConfigurationModuleCatalog catalog = new ConfigurationModuleCatalog() { Store = store };
			catalog.Load();

			Assert.Equal(0, catalog.Modules.Count());
		}

		[Fact]
		public void EnumeratesThreeModulesWithDependencies()
		{
			var module1 = new ModuleConfigurationElement( "Module1.dll", "Test.Module1", "Module1", false )
			{
				Dependencies = new ModuleDependencyCollection( new[] { new ModuleDependencyConfigurationElement( "Module2" ) } )
			};

			var module2 = new ModuleConfigurationElement( "Module2.dll", "Test.Module2", "Module2", false )
			{
				Dependencies = new ModuleDependencyCollection( new[] { new ModuleDependencyConfigurationElement( "Module3" ) } )
			};

			var module3 = new ModuleConfigurationElement("Module3.dll", "Test.Module3", "Module3", false);
			var catalog = new ConfigurationModuleCatalog { Store = new MockConfigurationStore { Modules = new[] { module3, module2, module1 } } };
			catalog.Load();

			var modules = catalog.Modules;

			Assert.Equal(3, modules.Count());
			Assert.True(modules.Any(module => module.ModuleName == "Module1"));
			Assert.True(modules.Any(module => module.ModuleName == "Module2"));
			Assert.True(modules.Any(module => module.ModuleName == "Module3"));
		}

		[Fact]
		public void EnumerateThrowsIfDuplicateNames()
		{
			MockConfigurationStore store = new MockConfigurationStore();
			var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);
			var module2 = new ModuleConfigurationElement("Module2.dll", "Test.Module2", "Module1", false);
			Assert.Throws<ConfigurationErrorsException>( () => store.Modules = new[] { module2, module1 } );
			// ConfigurationModuleCatalog catalog = new ConfigurationModuleCatalog { Store = store };
		}

		[Fact]
		public void EnumerateNotThrowsIfDuplicateAssemblyFile()
		{
			MockConfigurationStore store = new MockConfigurationStore();
			var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);
			var module2 = new ModuleConfigurationElement("Module1.dll", "Test.Module2", "Module2", false);
			store.Modules = new[] { module2, module1 };
			ConfigurationModuleCatalog catalog = new ConfigurationModuleCatalog() { Store = store };
			catalog.Load();

			Assert.Equal(2, catalog.Modules.Count());
		}

		[Fact]
		public void GetStartupLoadedModulesDoesNotRetrieveOnDemandLoaded()
		{
			MockConfigurationStore store = new MockConfigurationStore();
			var module1 = new ModuleConfigurationElement("Module1.dll", "Test.Module1", "Module1", false);
			store.Modules = new[] { module1 };

			ConfigurationModuleCatalog catalog = new ConfigurationModuleCatalog() { Store = store };
			catalog.Load();

			Assert.Equal(1, catalog.Modules.Count());
			Assert.Equal(0, catalog.Modules.OfType<DynamicModuleInfo>().Count(m => m.InitializationMode != InitializationMode.OnDemand));
		}

		[Fact]
		public void GetModulesNotThrownIfModuleSectionIsNotDeclared()
		{
			MockNullConfigurationStore store = new MockNullConfigurationStore();

			ConfigurationModuleCatalog catalog = new ConfigurationModuleCatalog() { Store = store };
			catalog.Load();

			var modules = catalog.Modules;

			Assert.NotNull(modules);
			Assert.Equal(0, modules.Count());
		}

		class MockNullConfigurationStore : IConfigurationStore
		{
			public ModulesConfigurationSection RetrieveModuleConfigurationSection()
			{
				return null;
			}
		}
	}
}
