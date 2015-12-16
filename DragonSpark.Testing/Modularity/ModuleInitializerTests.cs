using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Testing.TestObjects.Modules;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Modularity
{
	/// <summary>
	/// Summary description for ModuleInitializerTests
	/// </summary>
	public class ModuleInitializerTests
	{
		[Fact]
		public void NullContainerThrows()
		{
			Assert.Throws<ArgumentNullException>( () => new ModuleInitializer( null, new MockMessageLogger() ) );
		}

		[Fact]
		public void NullLoggerThrows()
		{
			Assert.Throws<ArgumentNullException>( () => new ModuleInitializer(new MockContainerAdapter(), null ) );
		}

		[Fact]
		public void InitializationExceptionsAreWrapped()
		{
			var moduleInfo = CreateModuleInfo( typeof(ExceptionThrowingModule) );

			var loader = new ModuleInitializer( new MockContainerAdapter(), new MockMessageLogger() );

			Assert.Throws<ModuleInitializeException>( () => loader.Initialize( moduleInfo ) );
		}

		[Fact]
		public void ShouldResolveModuleAndInitializeSingleModule()
		{
			IServiceLocator containerFacade = new MockContainerAdapter();
			var service = new ModuleInitializer(containerFacade, new MockMessageLogger());
			FirstTestModule.wasInitializedOnce = false;
			var info = CreateModuleInfo(typeof(FirstTestModule));
			service.Initialize(info);
			Assert.True(FirstTestModule.wasInitializedOnce);
		}

		[Fact]
		public void ShouldLogModuleInitializeErrorsAndContinueLoading()
		{
			IServiceLocator containerFacade = new MockContainerAdapter();
			var logger = new MockMessageLogger();
			var service = new CustomModuleInitializerService(containerFacade, logger);
			var invalidModule = CreateModuleInfo(typeof(InvalidModule));

			Assert.False(service.HandleModuleInitializeErrorCalled);
			service.Initialize(invalidModule);
			Assert.True(service.HandleModuleInitializeErrorCalled);
		}

		[Fact]
		public void ShouldLogModuleInitializationError()
		{
			IServiceLocator containerFacade = new MockContainerAdapter();
			var logger = new MockMessageLogger();
			var service = new ModuleInitializer(containerFacade, logger);
			ExceptionThrowingModule.wasInitializedOnce = false;
			var exceptionModule = CreateModuleInfo(typeof(ExceptionThrowingModule));

			try
			{
				service.Initialize(exceptionModule);
			}
			catch (ModuleInitializeException)
			{
			}

			Assert.NotNull(logger.LastMessage);
			Assert.Contains("ExceptionThrowingModule", logger.LastMessage);
		}

		[Fact]
		public void ShouldThrowExceptionIfBogusType()
		{
			var moduleInfo = new ModuleInfo("TestModule", "BadAssembly.BadType");

			ModuleInitializer loader = new ModuleInitializer(new MockContainerAdapter(), new MockMessageLogger());

			try
			{
				loader.Initialize(moduleInfo);
				throw new InvalidOperationException("Did not throw exception");
			}
			catch (ModuleInitializeException ex)
			{
				Assert.Contains("BadAssembly.BadType", ex.Message);
			}
		}

		static ModuleInfo CreateModuleInfo(Type type, params string[] dependsOn)
		{
			ModuleInfo moduleInfo = new ModuleInfo(type.Name, type.AssemblyQualifiedName);
			moduleInfo.DependsOn.AddRange(dependsOn);
			return moduleInfo;
		}

		static class ModuleLoadTracker
		{
			public static readonly Stack<Type> ModuleLoadStack = new Stack<Type>();
		}

		class FirstTestModule : IModule
		{
			public static bool wasInitializedOnce;

			public void Initialize()
			{
				wasInitializedOnce = true;
				ModuleLoadTracker.ModuleLoadStack.Push(GetType());
			}

			public void Load()
			{}
		}

		public class SecondTestModule : IModule
		{
			public void Initialize()
			{
				ModuleLoadTracker.ModuleLoadStack.Push(GetType());
			}

			void IModule.Load()
			{}
		}

		public class DependantModule : IModule
		{
			public void Initialize()
			{
				ModuleLoadTracker.ModuleLoadStack.Push(GetType());
			}

			public void Load()
			{}
		}

		public class DependencyModule : IModule
		{
			
			public void Initialize()
			{
				ModuleLoadTracker.ModuleLoadStack.Push(GetType());
			}

			public void Load()
			{}
		}

		class ExceptionThrowingModule : IModule
		{
			public static bool wasInitializedOnce;
			
			public void Initialize()
			{
				throw new InvalidOperationException("Intialization can't be performed");
			}

			public void Load()
			{}
		}

		class InvalidModule { }

		class CustomModuleInitializerService : ModuleInitializer
		{
			public bool HandleModuleInitializeErrorCalled;

			public CustomModuleInitializerService(IServiceLocator containerFacade, IMessageLogger messageLogger) : base(containerFacade, messageLogger)
			{}

			public override void HandleModuleInitializationError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
			{
				HandleModuleInitializeErrorCalled = true;
			}
		}

		public class Module1 : IModule { void IModule.Initialize() { } void IModule.Load() { } }
		public class Module2 : IModule { void IModule.Initialize() { } void IModule.Load() { } }
		public class Module3 : IModule { void IModule.Initialize() { } void IModule.Load() { } }
		public class Module4 : IModule { void IModule.Initialize() { } void IModule.Load() { } }
	}
}