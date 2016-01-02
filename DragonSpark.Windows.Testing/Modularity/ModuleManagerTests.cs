using System;
using System.Collections.Generic;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Windows.Modularity;
using DragonSpark.Windows.Testing.TestObjects.Modules;
using Moq;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class ModuleManagerTests
	{
		[Fact]
		public void NullLoaderThrows()
		{
			Assert.Throws<ArgumentNullException>( () => new ModuleManager( null, new MockModuleCatalog(), new MockMessageLogger(), new MockModuleTypeLoader() ) );
		}

		[Fact]
		public void NullCatalogThrows()
		{
			Assert.Throws<ArgumentNullException>( () => new ModuleManager(new MockModuleInitializer(), null, new MockMessageLogger(), new MockModuleTypeLoader() ) );
		}

		[Fact]
		public void NullLoggerThrows()
		{
			Assert.Throws<ArgumentNullException>( () => new ModuleManager(new MockModuleInitializer(), new MockModuleCatalog(), null, new MockModuleTypeLoader()) );
		}       

		[Fact]
		public void ShouldInvokeRetrieverForModules()
		{
			var loader = new MockModuleInitializer();
			var moduleInfo = CreateModuleInfo("needsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new MockModuleCatalog { Modules = { moduleInfo } };

			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManager( loader, catalog, new MockMessageLogger(), moduleTypeLoader ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader } };



			manager.Run();

			Assert.True(moduleTypeLoader.LoadedModules.Contains(moduleInfo));
		}

		[Fact]
		public void ShouldInitializeModulesOnRetrievalCompleted()
		{
			var loader = new MockModuleInitializer();
			var backgroungModuleInfo = CreateModuleInfo("NeedsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new MockModuleCatalog { Modules = { backgroungModuleInfo } };
			ModuleManager manager = new ModuleManager(loader, catalog, new MockMessageLogger(), new MockModuleTypeLoader());
			var moduleTypeLoader = new MockModuleTypeLoader();
			manager.ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader };            
			Assert.False(loader.InitializeCalled);

			manager.Run();

			Assert.True(loader.InitializeCalled);
			Assert.Equal(1, loader.InitializedModules.Count);
			Assert.Equal(backgroungModuleInfo, loader.InitializedModules[0]);
		}

		[Fact]
		public void ShouldInitializeModuleOnDemand()
		{
			var loader = new MockModuleInitializer();
			var onDemandModule = CreateModuleInfo("NeedsRetrieval", InitializationMode.OnDemand);
			var catalog = new MockModuleCatalog { Modules = { onDemandModule } };
			var moduleRetriever = new MockModuleTypeLoader();
			var manager = new ModuleManager( loader, catalog, new MockMessageLogger(), moduleRetriever ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleRetriever } };
			manager.Run();

			Assert.False(loader.InitializeCalled);
			Assert.Equal(0, moduleRetriever.LoadedModules.Count);

			manager.LoadModule("NeedsRetrieval");

			Assert.Equal(1, moduleRetriever.LoadedModules.Count);
			Assert.True(loader.InitializeCalled);
			Assert.Equal(1, loader.InitializedModules.Count);
			Assert.Equal(onDemandModule, loader.InitializedModules[0]);
		}

		[Fact]
		public void InvalidOnDemandModuleNameThrows()
		{
			var loader = new MockModuleInitializer();

			var catalog = new MockModuleCatalog { Modules = new List<ModuleInfo> { CreateModuleInfo("Missing", InitializationMode.OnDemand) } };

			var moduleTypeLoader = new MockModuleTypeLoader();
			ModuleManager manager = new ModuleManager( loader, catalog, new MockMessageLogger(), moduleTypeLoader ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader } };

			manager.Run();

			Assert.Throws<ModuleNotFoundException>( () => manager.LoadModule("NonExistent") );
		}

		[Fact]
		public void EmptyOnDemandModuleReturnedThrows()
		{
			var loader = new MockModuleInitializer();

			var catalog = new MockModuleCatalog { CompleteListWithDependencies = modules => new List<ModuleInfo>() };
			var moduleRetriever = new MockModuleTypeLoader();
			var manager = new ModuleManager( loader, catalog, new MockMessageLogger(), moduleRetriever ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleRetriever } };
			manager.Run();

			Assert.Throws<ModuleNotFoundException>( () => manager.LoadModule("NullModule") );
		}

		[Fact]
		public void ShouldNotLoadTypeIfModuleInitialized()
		{
			var loader = new MockModuleInitializer();
			var alreadyPresentModule = CreateModuleInfo(typeof(MockModule), InitializationMode.WhenAvailable);
			alreadyPresentModule.State = ModuleState.ReadyForInitialization;
			var catalog = new MockModuleCatalog { Modules = { alreadyPresentModule } };
			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManager( loader, catalog, new MockMessageLogger(), moduleTypeLoader ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader } };

			manager.Run();

			Assert.False(moduleTypeLoader.LoadedModules.Contains(alreadyPresentModule));
			Assert.True(loader.InitializeCalled);
			Assert.Equal(1, loader.InitializedModules.Count);
			Assert.Equal(alreadyPresentModule, loader.InitializedModules[0]);
		}

		[Fact]
		public void ShouldNotLoadSameModuleTwice()
		{
			var loader = new MockModuleInitializer();
			var onDemandModule = CreateModuleInfo(typeof(MockModule), InitializationMode.OnDemand);
			var catalog = new MockModuleCatalog { Modules = { onDemandModule } };
			var manager = new ModuleManager(loader, catalog, new MockMessageLogger(), new MockModuleTypeLoader());
			manager.Run();
			manager.LoadModule("MockModule");
			loader.InitializeCalled = false;
			manager.LoadModule("MockModule");

			Assert.False(loader.InitializeCalled);
		}

		[Fact]
		public void ShouldNotLoadModuleThatNeedsRetrievalTwice()
		{
			var loader = new MockModuleInitializer();
			var onDemandModule = CreateModuleInfo("ModuleThatNeedsRetrieval", InitializationMode.OnDemand);
			var catalog = new MockModuleCatalog { Modules = { onDemandModule } };
			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManager( loader, catalog, new MockMessageLogger(), moduleTypeLoader ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader } };
			manager.Run();
			manager.LoadModule("ModuleThatNeedsRetrieval");
			moduleTypeLoader.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(onDemandModule, null));
			loader.InitializeCalled = false;

			manager.LoadModule("ModuleThatNeedsRetrieval");

			Assert.False(loader.InitializeCalled);
		}

		[Fact]
		public void ShouldCallValidateCatalogBeforeGettingGroupsFromCatalog()
		{
			var loader = new MockModuleInitializer();
			var catalog = new MockModuleCatalog();
			var manager = new ModuleManager(loader, catalog, new MockMessageLogger(), new MockModuleTypeLoader());
			bool validateCatalogCalled = false;
			bool getModulesCalledBeforeValidate = false;

			catalog.ValidateCatalog = () => validateCatalogCalled = true;
			catalog.CompleteListWithDependencies = f =>
													 {
														 if (!validateCatalogCalled)
														 {
															 getModulesCalledBeforeValidate = true;
														 }

														 return null;
													 };
			manager.Run();

			Assert.True(validateCatalogCalled);
			Assert.False(getModulesCalledBeforeValidate);
		}

		[Fact]
		public void ShouldNotInitializeIfDependenciesAreNotMet()
		{
			var loader = new MockModuleInitializer();
			var requiredModule = CreateModuleInfo("ModuleThatNeedsRetrieval1", InitializationMode.WhenAvailable);
			requiredModule.ModuleName = "RequiredModule";
			var dependantModuleInfo = CreateModuleInfo("ModuleThatNeedsRetrieval2", InitializationMode.WhenAvailable, "RequiredModule");

			var catalog = new MockModuleCatalog
			{
				Modules = { requiredModule, dependantModuleInfo },
				GetDependentModules = m => new[] { requiredModule }
			};

			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManager( loader, catalog, new MockMessageLogger(), moduleTypeLoader ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader } };

			manager.Run();

			moduleTypeLoader.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(dependantModuleInfo, null));            

			Assert.False(loader.InitializeCalled);
			Assert.Equal(0, loader.InitializedModules.Count);
		}

		[Fact]
		public void ShouldInitializeIfDependenciesAreMet()
		{
			var initializer = new MockModuleInitializer();
			var requiredModule = CreateModuleInfo("ModuleThatNeedsRetrieval1", InitializationMode.WhenAvailable);
			requiredModule.ModuleName = "RequiredModule";
			var dependantModuleInfo = CreateModuleInfo("ModuleThatNeedsRetrieval2", InitializationMode.WhenAvailable, "RequiredModule");

			var catalog = new MockModuleCatalog { Modules = { requiredModule, dependantModuleInfo } };
			catalog.GetDependentModules = delegate(ModuleInfo module)
											  {
												  if (module == dependantModuleInfo)
													  return new[] { requiredModule };
												  else
													  return null;
											  };

			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManager( initializer, catalog, new MockMessageLogger(), moduleTypeLoader ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader } };

			manager.Run();

			Assert.True(initializer.InitializeCalled);
			Assert.Equal(2, initializer.InitializedModules.Count);
		}

		[Fact]
		public void ShouldThrowOnRetrieverErrorAndWrapException()
		{
			var loader = new MockModuleInitializer();
			var moduleInfo = CreateModuleInfo("NeedsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new MockModuleCatalog { Modules = { moduleInfo } };
			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManager(loader, catalog, new MockMessageLogger(), moduleTypeLoader);
			
			var retrieverException = new Exception();
			moduleTypeLoader.LoadCompletedError = retrieverException;

			manager.ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader };            
			Assert.False(loader.InitializeCalled);

			try
			{
				manager.Run();
			}
			catch (Exception ex)
			{
				Assert.IsType<ModuleTypeLoadingException>(ex);
				Assert.Equal(moduleInfo.ModuleName, ((ModularityException)ex).ModuleName);
				Assert.Contains(moduleInfo.ModuleName, ex.Message);
				Assert.Same(retrieverException, ex.InnerException);
				return;
			}

			throw new InvalidOperationException("Exception not thrown.");
		}

		[Fact]
		public void ShouldThrowIfNoRetrieverCanRetrieveModule()
		{
			var loader = new MockModuleInitializer();
			var catalog = new MockModuleCatalog { Modules = { CreateModuleInfo("ModuleThatNeedsRetrieval", InitializationMode.WhenAvailable) } };
			var typeLoader = new MockModuleTypeLoader() { CanLoadModuleTypeReturnValue = false };
			var manager = new ModuleManager( loader, catalog, new MockMessageLogger(), typeLoader ) { ModuleTypeLoaders = new List<IModuleTypeLoader> { typeLoader } };
			Assert.Throws<ModuleTypeLoaderNotFoundException>( () => manager.Run() );
		}

		[Fact]
		public void ShouldLogMessageOnModuleRetrievalError()
		{
			var loader = new MockModuleInitializer();
			var moduleInfo = CreateModuleInfo("ModuleThatNeedsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new MockModuleCatalog { Modules = { moduleInfo } };
			var logger = new MockMessageLogger();
			var moduleTypeLoader = new MockModuleTypeLoader();
			ModuleManager manager = new ModuleManager(loader, catalog, logger, moduleTypeLoader);
			moduleTypeLoader.LoadCompletedError = new Exception();
			manager.ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader };

			try
			{
				manager.Run();
			}
			catch
			{
				// Ignore all errors to make sure logger is called even if errors thrown.
			}

			Assert.NotNull(logger.LastMessage);
			Assert.Contains("ModuleThatNeedsRetrieval", logger.LastMessage);
			Assert.Equal("Exception", logger.LastMessageCategory);
		}

		[Fact]
		public void OnModuleRetrievalErrorDoesNotThrow()
		{
			var loader = new MockModuleInitializer();
			var moduleInfo = CreateModuleInfo("ModuleThatNeedsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new MockModuleCatalog { Modules = { moduleInfo } };
			var logger = new MockMessageLogger();
			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManagerExtended(loader, catalog, logger, moduleTypeLoader);
			moduleTypeLoader.LoadCompletedError = new Exception();
			manager.ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader };

			Assert.Null( manager.Handled );
			manager.Run();

			Assert.NotNull( logger.LastMessage );
			Assert.Contains( "ModuleThatNeedsRetrieval", logger.LastMessage );
			Assert.Equal( "Exception", logger.LastMessageCategory );
			Assert.NotNull( manager.Handled );
		}

		class ModuleManagerExtended : ModuleManager
		{
			public Exception Handled { get; private set; }
			

			public ModuleManagerExtended( IModuleInitializer moduleInitializer, IModuleCatalog moduleCatalog, IMessageLogger messageLoggerFacade, IModuleTypeLoader loader ) : base( moduleInitializer, moduleCatalog, messageLoggerFacade, loader )
			{}

			protected override void HandleModuleTypeLoadingError( ModuleInfo moduleInfo, Exception exception )
			{
				try
				{
					base.HandleModuleTypeLoadingError( moduleInfo, exception );
				}
				catch ( Exception e )
				{
					Handled = e;
				}
			}
		}

		[Fact]
		public void ShouldNotLogMessageOnModuleRetrievalErrorOnHandled()
		{
			var loader = new MockModuleInitializer();
			var moduleInfo = CreateModuleInfo("ModuleThatNeedsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new MockModuleCatalog { Modules = { moduleInfo } };
			var logger = new MockMessageLogger();
			var moduleTypeLoader = new MockModuleTypeLoader();
			var manager = new ModuleManager(loader, catalog, logger, moduleTypeLoader);
			manager.LoadModuleCompleted += ( sender, args ) => args.IsErrorHandled = true;
			moduleTypeLoader.LoadCompletedError = new Exception();
			manager.ModuleTypeLoaders = new List<IModuleTypeLoader> { moduleTypeLoader };

			manager.Run();

			Assert.Null( logger.LastMessage );
			Assert.DoesNotContain( "ModuleThatNeedsRetrieval", logger.LastMessage );
			Assert.NotEqual( "Exception", logger.LastMessageCategory );
		}

		[Fact]
		public void ShouldWorkIfModuleLoadsAnotherOnDemandModuleWhenInitializing()
		{
			var initializer = new StubModuleInitializer();
			var onDemandModule = CreateModuleInfo(typeof(MockModule), InitializationMode.OnDemand);
			onDemandModule.ModuleName = "OnDemandModule";
			var moduleThatLoadsOtherModule = CreateModuleInfo(typeof(MockModule), InitializationMode.WhenAvailable);
			var catalog = new MockModuleCatalog { Modules = { moduleThatLoadsOtherModule, onDemandModule } };
			ModuleManager manager = new ModuleManager(initializer, catalog, new MockMessageLogger(), new MockModuleTypeLoader());
			bool onDemandModuleWasInitialized = false;
			initializer.Initialize = m =>
									 {
										 if (m == moduleThatLoadsOtherModule)
										 {
											 manager.LoadModule("OnDemandModule");
										 }
										 else if (m == onDemandModule)
										 {
											 onDemandModuleWasInitialized = true;
										 }
									 };

			manager.Run();

			Assert.True(onDemandModuleWasInitialized);
		}

		[Fact]
		public void ModuleManagerIsDisposable()
		{
			Mock<IModuleInitializer> mockInit = new Mock<IModuleInitializer>(); 
			var moduleInfo = CreateModuleInfo("needsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new Mock<IModuleCatalog>();
			ModuleManager manager = new ModuleManager(mockInit.Object, catalog.Object, new MockMessageLogger(), new MockModuleTypeLoader());

			IDisposable disposableManager = manager as IDisposable;
			Assert.NotNull(disposableManager);
		}

		[Fact]
		public void DisposeDoesNotThrowWithNonDisposableTypeLoaders()
		{
			Mock<IModuleInitializer> mockInit = new Mock<IModuleInitializer>();
			var moduleInfo = CreateModuleInfo("needsRetrieval", InitializationMode.WhenAvailable);
			var catalog = new Mock<IModuleCatalog>();
			ModuleManager manager = new ModuleManager(mockInit.Object, catalog.Object, new MockMessageLogger(), new MockModuleTypeLoader());

			var mockTypeLoader = new Mock<IModuleTypeLoader>();
			manager.ModuleTypeLoaders = new List<IModuleTypeLoader> {mockTypeLoader.Object};

			manager.Dispose();
		}

		[Fact]
		public void DisposeCleansUpDisposableTypeLoaders()
		{
			Mock<IModuleInitializer> mockInit = new Mock<IModuleInitializer>();
			var catalog = new Mock<IModuleCatalog>();
			ModuleManager manager = new ModuleManager(mockInit.Object, catalog.Object, new MockMessageLogger(), new MockModuleTypeLoader());

			var mockTypeLoader = new Mock<IModuleTypeLoader>();
			var disposableMockTypeLoader = mockTypeLoader.As<IDisposable>();
			disposableMockTypeLoader.Setup(loader => loader.Dispose());

			manager.ModuleTypeLoaders = new List<IModuleTypeLoader> { mockTypeLoader.Object };

			manager.Dispose();

			disposableMockTypeLoader.Verify(loader => loader.Dispose(), Times.Once());
		}

		[Fact]
		public void DisposeDoesNotThrowWithMixedTypeLoaders()
		{
			Mock<IModuleInitializer> mockInit = new Mock<IModuleInitializer>();
			var catalog = new Mock<IModuleCatalog>();
			ModuleManager manager = new ModuleManager(mockInit.Object, catalog.Object, new MockMessageLogger(), new MockModuleTypeLoader());

			var mockTypeLoader1 = new Mock<IModuleTypeLoader>();

			var mockTypeLoader = new Mock<IModuleTypeLoader>();
			var disposableMockTypeLoader = mockTypeLoader.As<IDisposable>();
			disposableMockTypeLoader.Setup(loader => loader.Dispose());

			manager.ModuleTypeLoaders = new List<IModuleTypeLoader>() { mockTypeLoader1.Object, mockTypeLoader.Object };
			
			manager.Dispose();

			disposableMockTypeLoader.Verify(loader => loader.Dispose(), Times.Once());
		}
		static ModuleInfo CreateModuleInfo(string name, InitializationMode initializationMode, params string[] dependsOn)
		{
			var moduleInfo = new DynamicModuleInfo( name, name ) { InitializationMode = initializationMode };
			moduleInfo.DependsOn.AddRange(dependsOn);
			return moduleInfo;
		}

		static ModuleInfo CreateModuleInfo(Type type, InitializationMode initializationMode, params string[] dependsOn)
		{
			var moduleInfo = new DynamicModuleInfo( type.Name, type.AssemblyQualifiedName ) { InitializationMode = initializationMode };
			moduleInfo.DependsOn.AddRange(dependsOn);
			return moduleInfo;
		}
	}

	internal class MockModule : IModule
	{
		public void Initialize()
		{
			throw new System.NotImplementedException();
		}

		public void Load()
		{
			throw new NotImplementedException();
		}
	}

	internal class MockModuleCatalog : IModuleCatalog
	{
		public List<ModuleInfo> Modules = new List<ModuleInfo>();
		public Func<ModuleInfo, IEnumerable<ModuleInfo>> GetDependentModules;

		public Func<IEnumerable<ModuleInfo>, IEnumerable<ModuleInfo>> CompleteListWithDependencies;
		public Action ValidateCatalog;

		public void Initialize()
		{
			if (this.ValidateCatalog != null)
			{
				this.ValidateCatalog();
			}
		}

		IEnumerable<ModuleInfo> IModuleCatalog.Modules
		{
			get { return this.Modules; }
		}

		IEnumerable<ModuleInfo> IModuleCatalog.GetDependentModules(ModuleInfo moduleInfo)
		{
			if (GetDependentModules == null)
				return new List<ModuleInfo>();

			return GetDependentModules(moduleInfo);
		}

		IEnumerable<ModuleInfo> IModuleCatalog.CompleteListWithDependencies(IEnumerable<ModuleInfo> modules)
		{
			if (CompleteListWithDependencies != null)
				return CompleteListWithDependencies(modules);
			return modules;
		}


		public void AddModule(ModuleInfo moduleInfo)
		{
			this.Modules.Add(moduleInfo);
		}
	}

	internal class MockModuleInitializer : IModuleInitializer
	{
		public bool InitializeCalled;
		public List<ModuleInfo> InitializedModules = new List<ModuleInfo>();

		public void Initialize(ModuleInfo moduleInfo)
		{
			InitializeCalled = true;            
			this.InitializedModules.Add(moduleInfo);
		}
	}

	internal class StubModuleInitializer : IModuleInitializer
	{
		public Action<ModuleInfo> Initialize;

		void IModuleInitializer.Initialize(ModuleInfo moduleInfo)
		{
			this.Initialize(moduleInfo);
		}
	}

	internal class MockDelegateModuleInitializer : IModuleInitializer
	{
		readonly Action<ModuleInfo> LoadBody = info => {};

		public void Initialize(ModuleInfo moduleInfo)
		{
			LoadBody(moduleInfo);
		}
	}
}
