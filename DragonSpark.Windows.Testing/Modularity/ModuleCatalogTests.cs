using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Modularity;
using Xunit;
using InitializationMode = DragonSpark.Windows.Modularity.InitializationMode;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class ModuleCatalogTests
	{
		[Fact]
		public void CanCreateCatalogFromList()
		{
			var moduleInfo = new ModuleInfo("MockModule", "type");
			var moduleInfos = new List<ModuleInfo> { moduleInfo };

			var moduleCatalog = new ModuleCatalog(moduleInfos);

			Assert.Equal(1, moduleCatalog.Modules.Count());
			Assert.Equal(moduleInfo, moduleCatalog.Modules.ElementAt(0));
		}

		[Fact]
		public void CanGetDependenciesForModule()
		{
			// A <- B
			var moduleInfoA = CreateModuleInfo("A");
			var moduleInfoB = CreateModuleInfo("B", "A");
			List<ModuleInfo> moduleInfos = new List<ModuleInfo>
											   {
												   moduleInfoA
												   , moduleInfoB
											   };
			var moduleCatalog = new ModuleCatalog(moduleInfos);

			IEnumerable<ModuleInfo> dependentModules = moduleCatalog.GetDependentModules(moduleInfoB);

			Assert.Equal(1, dependentModules.Count());
			Assert.Equal(moduleInfoA, dependentModules.ElementAt(0));
		}

		[Fact]
		public void CanCompleteListWithTheirDependencies()
		{
			// A <- B <- C
			var moduleInfoA = CreateModuleInfo("A");
			var moduleInfoB = CreateModuleInfo("B", "A");
			var moduleInfoC = CreateModuleInfo("C", "B");
			var moduleInfoOrphan = CreateModuleInfo("X", "B");

			List<ModuleInfo> moduleInfos = new List<ModuleInfo>
											   {
												   moduleInfoA
												   , moduleInfoB
												   , moduleInfoC
												   , moduleInfoOrphan
											   };
			var moduleCatalog = new ModuleCatalog(moduleInfos);

			IEnumerable<ModuleInfo> dependantModules = moduleCatalog.CompleteListWithDependencies(new[] { moduleInfoC });

			Assert.Equal(3, dependantModules.Count());
			Assert.True(dependantModules.Contains(moduleInfoA));
			Assert.True(dependantModules.Contains(moduleInfoB));
			Assert.True(dependantModules.Contains(moduleInfoC));
		}

		[Fact]
		public void ShouldThrowOnCyclicDependency()
		{
			// A <- B <- C <- A
			var moduleInfoA = CreateModuleInfo("A", "C");
			var moduleInfoB = CreateModuleInfo("B", "A");
			var moduleInfoC = CreateModuleInfo("C", "B");

			List<ModuleInfo> moduleInfos = new List<ModuleInfo>
											   {
												   moduleInfoA
												   , moduleInfoB
												   , moduleInfoC
											   };
			Assert.Throws<CyclicDependencyFoundException>( () => new ModuleCatalog( moduleInfos ).Validate() );
		}

		[Fact]
		public void ShouldThrowOnDuplicateModule()
		{

			var moduleInfoA1 = CreateModuleInfo("A");
			var moduleInfoA2 = CreateModuleInfo("A");

			List<ModuleInfo> moduleInfos = new List<ModuleInfo>
											   {
												   moduleInfoA1
												   , moduleInfoA2
											   };
			Assert.Throws<DuplicateModuleException>( () => new ModuleCatalog( moduleInfos ).Validate() );
		}

		[Fact]
		public void ShouldThrowOnMissingDependency()
		{
			var moduleInfoA = CreateModuleInfo("A", "B");

			List<ModuleInfo> moduleInfos = new List<ModuleInfo>
											   {
												   moduleInfoA
											   };
			Assert.Throws<ModularityException>( () => new ModuleCatalog( moduleInfos ).Validate() );
		}

		[Fact]
		public void CanAddModules()
		{
			var catalog = new ModuleCatalog();

			catalog.AddModule(typeof(MockModule));

			Assert.Equal(1, catalog.Modules.Count());
			Assert.Equal("MockModule", catalog.Modules.First().ModuleName);
		}

		[Fact]
		public void CanAddGroups()
		{
			var catalog = new ModuleCatalog();

			ModuleInfo moduleInfo = new ModuleInfo();
			ModuleInfoGroup group = new ModuleInfoGroup { moduleInfo };
			catalog.Items.Add(group);

			Assert.Equal(1, catalog.Modules.Count());
			Assert.Same(moduleInfo, catalog.Modules.ElementAt(0));
		}

		[Fact]
		public void ShouldAggregateGroupsAndLooseModuleInfos()
		{
			var catalog = new ModuleCatalog();
			ModuleInfo moduleInfo1 = new ModuleInfo();
			ModuleInfo moduleInfo2 = new ModuleInfo();
			ModuleInfo moduleInfo3 = new ModuleInfo();

			catalog.Items.Add(new ModuleInfoGroup() { moduleInfo1 });
			catalog.Items.Add(new ModuleInfoGroup() { moduleInfo2 });
			catalog.AddModule(moduleInfo3);

			Assert.Equal(3, catalog.Modules.Count());
			Assert.True(catalog.Modules.Contains(moduleInfo1));
			Assert.True(catalog.Modules.Contains(moduleInfo2));
			Assert.True(catalog.Modules.Contains(moduleInfo3));
		}

		[Fact]
		public void CompleteListWithDependenciesThrowsWithNull()
		{
			var catalog = new ModuleCatalog();
			Assert.Throws<ArgumentNullException>( () => catalog.CompleteListWithDependencies(null) );
		}

		[Fact]
		public void LooseModuleIfDependentOnModuleInGroupThrows()
		{
			var catalog = new ModuleCatalog();
			catalog.Items.Add(new ModuleInfoGroup() { CreateModuleInfo("ModuleA") });
			catalog.AddModule(CreateModuleInfo("ModuleB", "ModuleA"));

			try
			{
				catalog.Validate();
			}
			catch (Exception ex)
			{
				Assert.IsAssignableFrom<ModularityException>(ex);
				Assert.Equal("ModuleB", ((ModularityException)ex).ModuleName);

				return;
			}

			Assert.True(false, "Exception not thrown.");
		}

		[Fact]
		public void ModuleInGroupDependsOnModuleInOtherGroupThrows()
		{
			var catalog = new ModuleCatalog();
			catalog.Items.Add(new ModuleInfoGroup { CreateModuleInfo("ModuleA") });
			catalog.Items.Add(new ModuleInfoGroup { CreateModuleInfo("ModuleB", "ModuleA") });

			try
			{
				catalog.Validate();
			}
			catch (Exception ex)
			{
				Assert.IsType<ModularityException>(ex);
				Assert.Equal("ModuleB", ((ModularityException)ex).ModuleName);

				return;
			}

			Assert.True( false, "Exception not thrown.");
		}

		[Fact]
		public void ShouldRevalidateWhenAddingNewModuleIfValidated()
		{
			var testableCatalog = new TestableModuleCatalog();
			testableCatalog.Items.Add(new ModuleInfoGroup() { CreateModuleInfo("ModuleA") });
			testableCatalog.Validate();
			testableCatalog.Items.Add(new ModuleInfoGroup() { CreateModuleInfo("ModuleB") });
			Assert.True(testableCatalog.ValidateCalled);
		}

		[Fact]
		public void ModuleInGroupCanDependOnModuleInSameGroup()
		{
			var catalog = new ModuleCatalog();
			var moduleA = CreateModuleInfo("ModuleA");
			var moduleB = CreateModuleInfo("ModuleB", "ModuleA");
			catalog.Items.Add(new ModuleInfoGroup()
								  {
									  moduleA,
									  moduleB
								  });

			var moduleBDependencies = catalog.GetDependentModules(moduleB);

			Assert.Equal(1, moduleBDependencies.Count());
			Assert.Equal(moduleA, moduleBDependencies.First());
		}

		[Fact]
		public void StartupModuleDependentOnAnOnDemandModuleThrows()
		{
			var catalog = new ModuleCatalog();
			var moduleOnDemand = CreateModuleInfo("ModuleA");
			moduleOnDemand.InitializationMode = InitializationMode.OnDemand;
			catalog.AddModule(moduleOnDemand);
			catalog.AddModule(CreateModuleInfo("ModuleB", "ModuleA"));

			try
			{
				catalog.Validate();
			}
			catch (Exception ex)
			{
				Assert.IsType<ModularityException>(ex);
				Assert.Equal("ModuleB", ((ModularityException)ex).ModuleName);

				return;
			}

			Assert.True( false, "Exception not thrown." );
		}

		[Fact]
		public void ShouldReturnInCorrectRetrieveOrderWhenCompletingListWithDependencies()
		{
			// A <- B <- C <- D,    C <- X
			var moduleA = CreateModuleInfo("A");
			var moduleB = CreateModuleInfo("B", "A");
			var moduleC = CreateModuleInfo("C", "B");
			var moduleD = CreateModuleInfo("D", "C");
			var moduleX = CreateModuleInfo("X", "C");

			var moduleCatalog = new ModuleCatalog();
			// Add the modules in random order
			moduleCatalog.AddModule(moduleB);
			moduleCatalog.AddModule(moduleA);
			moduleCatalog.AddModule(moduleD);
			moduleCatalog.AddModule(moduleX);
			moduleCatalog.AddModule(moduleC);

			var dependantModules = moduleCatalog.CompleteListWithDependencies(new[] { moduleD, moduleX }).ToList();

			Assert.Equal(5, dependantModules.Count);
			Assert.True(dependantModules.IndexOf(moduleA) < dependantModules.IndexOf(moduleB));
			Assert.True(dependantModules.IndexOf(moduleB) < dependantModules.IndexOf(moduleC));
			Assert.True(dependantModules.IndexOf(moduleC) < dependantModules.IndexOf(moduleD));
			Assert.True(dependantModules.IndexOf(moduleC) < dependantModules.IndexOf(moduleX));
		}

		[Theory, AutoData]
		public void Load( ModuleCatalog sut )
		{
			sut.Load();
			sut.Initialize();
		}

		/*public void CanLoadCatalogFromXaml()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "Prism.Wpf.Tests.Modularity.ModuleCatalogXaml.SimpleModuleCatalog.xaml" );
			var catalog = ModuleCatalog.CreateFromXaml(stream);
			Assert.NotNull(catalog);

			Assert.Equal(4, catalog.Modules.Count());
		}*/

		[Fact]
		public void ShouldLoadAndValidateOnInitialize()
		{
			var catalog = new TestableModuleCatalog();

			var testableCatalog = new TestableModuleCatalog();
			Assert.False(testableCatalog.LoadCalled);
			Assert.False(testableCatalog.ValidateCalled);

			testableCatalog.Initialize();
			Assert.True(testableCatalog.LoadCalled);
			Assert.True(testableCatalog.ValidateCalled);
			Assert.True(testableCatalog.LoadCalledFirst);
		}

		[Fact]
		public void ShouldNotLoadAgainIfInitializedCalledMoreThanOnce()
		{
			var catalog = new TestableModuleCatalog();

			var testableCatalog = new TestableModuleCatalog();
			Assert.False(testableCatalog.LoadCalled);
			Assert.False(testableCatalog.ValidateCalled);

			testableCatalog.Initialize();
			Assert.Equal(1, testableCatalog.LoadCalledCount);
			testableCatalog.Initialize();
			Assert.Equal(1, testableCatalog.LoadCalledCount);
		}

		[Fact]
		public void ShouldNotLoadAgainDuringInitialize()
		{
			var catalog = new TestableModuleCatalog();

			var testableCatalog = new TestableModuleCatalog();
			Assert.False(testableCatalog.LoadCalled);
			Assert.False(testableCatalog.ValidateCalled);

			testableCatalog.Load();
			Assert.Equal(1, testableCatalog.LoadCalledCount);
			testableCatalog.Initialize();
			Assert.Equal(1, testableCatalog.LoadCalledCount);
		}

		[Fact]
		public void ShouldAllowLoadToBeInvokedTwice()
		{
			var catalog = new TestableModuleCatalog();

			var testableCatalog = new TestableModuleCatalog();
			testableCatalog.Load();
			Assert.Equal<int>(1, testableCatalog.LoadCalledCount);
			testableCatalog.Load();
			Assert.Equal<int>(2, testableCatalog.LoadCalledCount);
		}

		[Fact]
		public void CanAddModule1()
		{
			ModuleCatalog catalog = new ModuleCatalog();

			catalog.AddModule("Module", "ModuleType", InitializationMode.OnDemand, "DependsOn1", "DependsOn2");

			Assert.Equal(1, catalog.Modules.Count());
			var moduleInfo = catalog.Modules.OfType<DynamicModuleInfo>().First();
			Assert.Equal("Module", moduleInfo.ModuleName);
			Assert.Equal("ModuleType", moduleInfo.ModuleType);
			Assert.Equal(InitializationMode.OnDemand, moduleInfo.InitializationMode);
			Assert.Equal(2, moduleInfo.DependsOn.Count);
			Assert.Equal("DependsOn1", moduleInfo.DependsOn[0]);
			Assert.Equal("DependsOn2", moduleInfo.DependsOn[1]);

		}

		[Fact]
		public void CanAddModule2()
		{
			ModuleCatalog catalog = new ModuleCatalog();

			catalog.AddModule("Module", "ModuleType", "DependsOn1", "DependsOn2");

			Assert.Equal(1, catalog.Modules.Count());
			var moduleInfo = catalog.Modules.First();
			Assert.Equal("Module", moduleInfo.ModuleName);
			Assert.Equal("ModuleType", moduleInfo.ModuleType);
			// Assert.Equal(InitializationMode.WhenAvailable, moduleInfo.InitializationMode);
			Assert.Equal(2, moduleInfo.DependsOn.Count);
			Assert.Equal("DependsOn1", moduleInfo.DependsOn[0]);
			Assert.Equal("DependsOn2", moduleInfo.DependsOn[1]);

		}

		[Fact]
		public void CanAddModule3()
		{
			ModuleCatalog catalog = new ModuleCatalog();

			catalog.AddModule(typeof(MockModule), InitializationMode.OnDemand, "DependsOn1", "DependsOn2");

			Assert.Equal(1, catalog.Modules.Count());
			var moduleInfo = catalog.Modules.OfType<DynamicModuleInfo>().First();
			Assert.Equal("MockModule", moduleInfo.ModuleName);
			Assert.Equal(typeof(MockModule).AssemblyQualifiedName, moduleInfo.ModuleType);
			Assert.Equal(InitializationMode.OnDemand, moduleInfo.InitializationMode);
			Assert.Equal(2, moduleInfo.DependsOn.Count);
			Assert.Equal("DependsOn1", moduleInfo.DependsOn[0]);
			Assert.Equal("DependsOn2", moduleInfo.DependsOn[1]);

		}

		[Fact]
		public void CanAddModule4()
		{
			ModuleCatalog catalog = new ModuleCatalog();

			catalog.AddModule(typeof(MockModule), "DependsOn1", "DependsOn2");

			Assert.Equal(1, catalog.Modules.Count());
			var moduleInfo = catalog.Modules.First();
			Assert.Equal("MockModule", moduleInfo.ModuleName);
			Assert.Equal(typeof(MockModule).AssemblyQualifiedName, moduleInfo.ModuleType);
			// Assert.Equal(InitializationMode.WhenAvailable, moduleInfo.InitializationMode);
			Assert.Equal(2, moduleInfo.DependsOn.Count);
			Assert.Equal("DependsOn1", moduleInfo.DependsOn[0]);
			Assert.Equal("DependsOn2", moduleInfo.DependsOn[1]);
		}

		[Fact]
		public void CanAddGroup()
		{
			ModuleCatalog catalog = new ModuleCatalog();

			catalog.Items.Add(new ModuleInfoGroup());

			catalog.AddGroup(InitializationMode.OnDemand, "Ref1",
							 new DynamicModuleInfo("M1", "T1"),
							 new DynamicModuleInfo("M2", "T2", "M1"));

			Assert.Equal(2, catalog.Modules.Count());

			var typed = catalog.Modules.OfType<DynamicModuleInfo>();
			var module1 = typed.First();
			var module2 = typed.Skip(1).First();


			Assert.Equal("M1", module1.ModuleName);
			Assert.Equal("T1", module1.ModuleType);
			Assert.Equal("Ref1", module1.Ref);
			Assert.Equal(InitializationMode.OnDemand, module1.InitializationMode);

			Assert.Equal("M2", module2.ModuleName);
			Assert.Equal("T2", module2.ModuleType);
			Assert.Equal("Ref1", module2.Ref);
			Assert.Equal(InitializationMode.OnDemand, module2.InitializationMode);
		}

		class TestableModuleCatalog : ModuleCatalog
		{
			public bool ValidateCalled { get; set; }
			public bool LoadCalledFirst { get; set; }
			public bool LoadCalled
			{
				get { return LoadCalledCount > 0; }
			}
			public int LoadCalledCount { get; set; }

			public override void Validate()
			{
				ValidateCalled = true;
				Validated = true;
			}

			protected override void InnerLoad()
			{
				if (ValidateCalled == false && !LoadCalled)
					LoadCalledFirst = true;

				LoadCalledCount++;
			}

		}

		private static DynamicModuleInfo CreateModuleInfo(string name, params string[] dependsOn)
		{
			var moduleInfo = new DynamicModuleInfo(name, name);
			moduleInfo.DependsOn.AddRange(dependsOn);
			return moduleInfo;
		}
	}
}
