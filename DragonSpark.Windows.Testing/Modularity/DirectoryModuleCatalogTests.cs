using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Testing.Framework;
using DragonSpark.Windows.Modularity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class DirectoryModuleCatalogTests : Tests
	{
		internal const string ModulesDirectory1 = @".\DynamicModules\MocksModules1",
			ModulesDirectory2 = @".\DynamicModules\AttributedModules",
			ModulesDirectory3 = @".\DynamicModules\DependantModules",
			ModulesDirectory4 = @".\DynamicModules\MocksModules2",
			ModulesDirectory5 = @".\DynamicModules\ModulesMainDomain\",
			InvalidModulesDirectory = @".\Modularity\Invalid";

		public DirectoryModuleCatalogTests( ITestOutputHelper output ) : base( output )
		{
			CleanUp();
		}

		static void CleanUp()
		{
			var items = new[] { ModulesDirectory1, ModulesDirectory2, ModulesDirectory3, ModulesDirectory4, ModulesDirectory5, InvalidModulesDirectory };
			items.Each( CompilerHelper.CleanUpDirectory );
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			CleanUp();
		}

		[Fact]
		public void NullPathThrows()
		{
			var catalog = new DirectoryModuleCatalog { ModulePath = null };
			Assert.Throws<InvalidOperationException>( new Action( catalog.Load ) );
		}

		[Fact]
		public void EmptyPathThrows()
		{
			var catalog = new DirectoryModuleCatalog { ModulePath = string.Empty };
			Assert.Throws<InvalidOperationException>( new Action( catalog.Load ) );
		}

		[Fact]
		public void NonExistentPathThrows()
		{
			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog { ModulePath = "NonExistentPath" };
			Assert.Throws<InvalidOperationException>( new Action( catalog.Load ) );
		}

		[Fact]
		public void ShouldReturnAListOfModuleInfo()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
									   ModulesDirectory1 + @"\MockModuleA.dll");


			var catalog = new DirectoryModuleCatalog { ModulePath = ModulesDirectory1 };
			catalog.Load();

			var modules = catalog.Modules.ToArray();

			Assert.NotNull(modules);
			Assert.Equal(1, modules.Length);
			var info = modules.OfType<DynamicModuleInfo>().First();
			Assert.NotNull(info.Ref);
			Assert.StartsWith(Uri.UriSchemeFile, info.Ref);
			Assert.True(info.Ref.Contains(@"MockModuleA.dll"));
			Assert.NotNull(info.ModuleType);
			Assert.Contains("DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA", info.ModuleType);
		}

		[Fact]
		public void ShouldNotThrowWithNonValidDotNetAssembly()
		{
			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog { ModulePath = InvalidModulesDirectory };
			try
			{
				catalog.Load();
			}
			catch (Exception)
			{
				Assert.True(false, "Should not have thrown.");
			}
			
			var modules = catalog.Modules.ToArray();
			Assert.NotNull(modules);
			Assert.Equal(0, modules.Length);
		}

		[Fact]
		// [DeploymentItem(@"Modularity\NotAValidDotNetDll.txt.dll", InvalidModulesDirectory)]
		public void LoadsValidAssembliesWhenInvalidDllsArePresent()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
									   InvalidModulesDirectory + @"\MockModuleA.dll");

			var catalog = new DirectoryModuleCatalog { ModulePath = InvalidModulesDirectory };
			try
			{
				catalog.Load();
			}
			catch (Exception)
			{
				Assert.True(false, "Should not have thrown.");
			}

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.NotNull(modules);
			Assert.Equal(1, modules.Length);
			var info = modules.OfType<DynamicModuleInfo>().First();
			Assert.NotNull(info.Ref);
			Assert.StartsWith(Uri.UriSchemeFile, info.Ref);
			Assert.True(info.Ref.Contains(@"MockModuleA.dll"));
			Assert.NotNull(info.ModuleType);
			Assert.Contains("DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA", info.ModuleType);
		}

		[Fact]
		public void ShouldNotThrowWithLoadFromByteAssemblies()
		{
			CompilerHelper.CleanUpDirectory(@".\CompileOutput\");
			CompilerHelper.CleanUpDirectory(@".\IgnoreLoadFromByteAssembliesTestDir\");
			var results = CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
													 @".\CompileOutput\MockModuleA.dll");

			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockAttributedModule.cs",
									   @".\IgnoreLoadFromByteAssembliesTestDir\MockAttributedModule.dll");

			string path = @".\IgnoreLoadFromByteAssembliesTestDir";

			AppDomain testDomain = null;
			try
			{
				testDomain = CreateAppDomain();
				RemoteDirectoryLookupCatalog remoteEnum = CreateRemoteDirectoryModuleCatalogInAppDomain(testDomain);

				remoteEnum.LoadDynamicEmittedModule();

				remoteEnum.LoadAssembliesByByte(@".\CompileOutput\MockModuleA.dll");

				IEnumerable<ModuleInfo> infos = remoteEnum.DoEnumeration(path);


				Assert.NotNull(
					infos.FirstOrDefault(x => x.ModuleType.IndexOf("DragonSpark.Windows.Testing.TestObjects.Modules.MockAttributedModule") >= 0)
					);
			}
			finally
			{
				if (testDomain != null)
					AppDomain.Unload(testDomain);
			}
		}

		[Fact]
		public void ShouldGetModuleNameFromAttribute()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockAttributedModule.cs",
									   ModulesDirectory2 + @"\MockAttributedModule.dll");


			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory2;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.Equal(1, modules.Length);
			Assert.Equal("TestModule", modules[0].ModuleName);
		}

		[Fact]
		public void ShouldGetDependantModulesFromAttribute()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockDependencyModule.cs",
									   ModulesDirectory3 + @"\DependencyModule.dll");

			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockDependantModule.cs",
									   ModulesDirectory3 + @"\DependantModule.dll");

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory3;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.Equal(2, modules.Length);
			var dependantModule = modules.First(module => module.ModuleName == "DependantModule");
			var dependencyModule = modules.First(module => module.ModuleName == "DependencyModule");
			Assert.NotNull(dependantModule);
			Assert.NotNull(dependencyModule);
			Assert.NotNull(dependantModule.DependsOn);
			Assert.Equal(1, dependantModule.DependsOn.Count);
			Assert.Equal(dependencyModule.ModuleName, dependantModule.DependsOn[0]);
		}

		[Fact]
		public void UseClassNameAsModuleNameWhenNotSpecifiedInAttribute()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
									   ModulesDirectory1 + @"\MockModuleA.dll");

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory1;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.NotNull(modules);
			Assert.Equal("MockModuleA", modules[0].ModuleName);
		}

		[Fact]
		public void ShouldDefaultInitializationModeToWhenAvailable()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
									   ModulesDirectory1 + @"\MockModuleA.dll");

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog { ModulePath = ModulesDirectory1 };
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.NotNull(modules);
			Assert.Equal(InitializationMode.WhenAvailable, modules.OfType<DynamicModuleInfo>().First().InitializationMode);
		}

		[Fact]
		public void ShouldGetOnDemandFromAttribute()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockAttributedModule.cs",
									   ModulesDirectory3 + @"\MockAttributedModule.dll");

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory3;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.Equal(1, modules.Length);
			Assert.Equal(InitializationMode.OnDemand, modules.OfType<DynamicModuleInfo>().First().InitializationMode);

		}

		[Fact]
		public void ShouldNotLoadAssembliesInCurrentAppDomain()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
									   ModulesDirectory4 + @"\MockModuleA.dll");

			var catalog = new DirectoryModuleCatalog() { ModulePath = ModulesDirectory4 };
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			// filtering out dynamic assemblies due to using a dynamic mocking framework.
			Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => !assembly.IsDynamic)
				.FirstOrDefault(assembly => assembly.Location.Equals(modules.OfType<DynamicModuleInfo>().First().Ref, StringComparison.InvariantCultureIgnoreCase));
			Assert.Null(loadedAssembly);
		}

		[Fact]
		public void ShouldNotGetModuleInfoForAnAssemblyAlreadyLoadedInTheMainDomain()
		{
			var assemblyPath = Assembly.GetCallingAssembly().Location;
			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory5;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.Equal(0, modules.Length);
		}

		[Fact]
		public void ShouldLoadAssemblyEvenIfTheyAreReferencingEachOther()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
									   ModulesDirectory4 + @"\MockModuleZZZ.dll");

			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleReferencingOtherModule.cs",
									   ModulesDirectory4 + @"\MockModuleReferencingOtherModule.dll", ModulesDirectory4 + @"\MockModuleZZZ.dll");

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory4;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.Equal(2, modules.Count());
		}

		//Disabled Warning	
		// 'System.Security.Policy.Evidence.Count' is obsolete: '
		// "Evidence should not be treated as an ICollection. Please use GetHostEnumerator and GetAssemblyEnumerator to 
		// iterate over the evidence to collect a count."'
#pragma warning disable 0618

		[Fact]
		public void CreateChildAppDomainHasParentEvidenceAndSetup()
		{
			/*TestableDirectoryModuleCatalog catalog = new TestableDirectoryModuleCatalog { ModulePath = ModulesDirectory4 };
			catalog.Load();*/
			var parentEvidence = new Evidence();
			var parentSetup = new AppDomainSetup { ApplicationName = "Test Parent" };
			var parentAppDomain = AppDomain.CreateDomain( "Parent", parentEvidence, parentSetup );
			var childDomain = ChildDomainFactory.Instance.Create( parentAppDomain );

			Assert.Equal(parentEvidence.Count, childDomain.Evidence.Count);
			Assert.Equal("Test Parent", childDomain.SetupInformation.ApplicationName);
			Assert.NotEqual(AppDomain.CurrentDomain.Evidence.Count, childDomain.Evidence.Count);
			Assert.NotEqual(AppDomain.CurrentDomain.SetupInformation.ApplicationName, childDomain.SetupInformation.ApplicationName);
		}
#pragma warning restore 0618

		[Fact]
		public void ShouldLoadFilesEvenIfDynamicAssemblyExists()
		{
			CompilerHelper.CleanUpDirectory(@".\CompileOutput\");
			CompilerHelper.CleanUpDirectory(@".\IgnoreDynamicGeneratedFilesTestDir\");
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockAttributedModule.cs",
									   @".\IgnoreDynamicGeneratedFilesTestDir\MockAttributedModule.dll");

			string path = @".\IgnoreDynamicGeneratedFilesTestDir";

			AppDomain testDomain = null;
			try
			{
				testDomain = CreateAppDomain();
				RemoteDirectoryLookupCatalog remoteEnum = CreateRemoteDirectoryModuleCatalogInAppDomain(testDomain);

				remoteEnum.LoadDynamicEmittedModule();

				IEnumerable<ModuleInfo> infos = remoteEnum.DoEnumeration(path);

				Assert.NotNull(
					infos.FirstOrDefault(x => x.ModuleType.IndexOf("DragonSpark.Windows.Testing.TestObjects.Modules.MockAttributedModule", StringComparison.Ordinal) >= 0)
					);
			}
			finally
			{
				if (testDomain != null)
					AppDomain.Unload(testDomain);
			}
		}

		[Fact]
		public void ShouldLoadAssemblyEvenIfIsExposingTypesFromAnAssemblyInTheGac()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockExposingTypeFromGacAssemblyModule.cs",
									   ModulesDirectory4 + @"\MockExposingTypeFromGacAssemblyModule.dll", @"System.Transactions.dll");

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory4;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();

			Assert.Equal(1, modules.Length);
		}

		[Fact]
		public void ShouldNotFailWhenAlreadyLoadedAssembliesAreAlsoFoundOnTargetDirectory()
		{
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockModuleA.cs",
									   ModulesDirectory1 + @"\MockModuleA.dll");

			string filename = typeof(DirectoryModuleCatalog).Assembly.Location;
			string destinationFileName = Path.Combine(ModulesDirectory1, Path.GetFileName(filename));
			File.Copy(filename, destinationFileName);

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory1;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();
			Assert.Equal(1, modules.Length);
		}

		[Fact]
		public void ShouldIgnoreAbstractClassesThatImplementIModule()
		{
			CompilerHelper.CleanUpDirectory(ModulesDirectory1);
			CompilerHelper.CompileFile(@"DragonSpark.Windows.Testing.TestObjects.Modules.MockAbstractModule.cs",
									 ModulesDirectory1 + @"\MockAbstractModule.dll");

			string filename = typeof(DirectoryModuleCatalog).Assembly.Location;
			string destinationFileName = Path.Combine(ModulesDirectory1, Path.GetFileName(filename));
			File.Copy(filename, destinationFileName);

			DirectoryModuleCatalog catalog = new DirectoryModuleCatalog();
			catalog.ModulePath = ModulesDirectory1;
			catalog.Load();

			ModuleInfo[] modules = catalog.Modules.ToArray();
			Assert.Equal(1, modules.Length);
			Assert.Equal("MockInheritingModule", modules[0].ModuleName);

			CompilerHelper.CleanUpDirectory(ModulesDirectory1);
		}

		static AppDomain CreateAppDomain()
		{
			Evidence evidence = AppDomain.CurrentDomain.Evidence;
			AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;

			return AppDomain.CreateDomain("TestDomain", evidence, setup);
		}

		static RemoteDirectoryLookupCatalog CreateRemoteDirectoryModuleCatalogInAppDomain(AppDomain testDomain)
		{
			Type remoteEnumType = typeof(RemoteDirectoryLookupCatalog);

			var remoteEnum = (RemoteDirectoryLookupCatalog)testDomain.CreateInstanceFrom(
				remoteEnumType.Assembly.Location, remoteEnumType.FullName).Unwrap();
			return remoteEnum;
		}

		/*class TestableDirectoryModuleCatalog : DirectoryModuleCatalog
		{
			public new AppDomain BuildChildDomain(AppDomain currentDomain)
			{
				return base.BuildChildDomain(currentDomain);
			}
		}*/

		class RemoteDirectoryLookupCatalog : MarshalByRefObject
		{
			public void LoadAssembliesByByte(string assemblyPath)
			{
				byte[] assemblyBytes = File.ReadAllBytes(assemblyPath);
				AppDomain.CurrentDomain.Load(assemblyBytes);
			}

			public IEnumerable<ModuleInfo> DoEnumeration(string path)
			{
				var catalog = new DirectoryModuleCatalog { ModulePath = path };
				catalog.Load();
				return catalog.Modules.ToArray();
			}

			public void LoadDynamicEmittedModule()
			{
				// create a dynamic assembly and module 
				AssemblyName assemblyName = new AssemblyName();
				assemblyName.Name = "DynamicBuiltAssembly";
				AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
				ModuleBuilder module = assemblyBuilder.DefineDynamicModule("DynamicBuiltAssembly.dll");

				// create a new type
				TypeBuilder typeBuilder = module.DefineType("DynamicBuiltType", TypeAttributes.Public | TypeAttributes.Class);

				// Create the type
				Type helloWorldType = typeBuilder.CreateType();

			}
		}
	}
}
