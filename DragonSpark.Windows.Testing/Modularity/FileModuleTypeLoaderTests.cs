using System;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Windows.Modularity;
using Moq;
using Xunit;
using ModuleTypeLoader = DragonSpark.Windows.Modularity.ModuleTypeLoader;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class FileModuleTypeLoaderTests
	{
		[Fact]
		public void CanRetrieveModule()
		{
			var assemblyResolver = new MockAssemblyResolver();
			var retriever = new ModuleTypeLoader(assemblyResolver);
			string assembly = CompilerHelper.GenerateDynamicModule("FileModuleA", null);
			string assemblyRef = "file://" + assembly;
			var fileModuleInfo = CreateModuleInfo(assemblyRef, "TestModules.FileModuleAClass", "ModuleA", true, null);

			bool loadCompleted = false;
			retriever.LoadModuleCompleted += ( sender, e ) => loadCompleted = true;

			retriever.LoadModuleType(fileModuleInfo);

			Assert.True(loadCompleted);
			Assert.Equal(assemblyRef, assemblyResolver.LoadAssemblyFromArgument);
		}

		[Fact]
		public void ShouldReturnErrorToCallback()
		{
			var assemblyResolver = new MockAssemblyResolver();
			var retriever = new ModuleTypeLoader(assemblyResolver);
			var fileModuleInfo = CreateModuleInfo("NonExistentFile.dll", "NonExistentModule", "NonExistent", true, null);

			assemblyResolver.ThrowOnLoadAssemblyFrom = true;
			Exception resultException = null;

			bool loadCompleted = false;
			retriever.LoadModuleCompleted += delegate(object sender, LoadModuleCompletedEventArgs e)
			{
				loadCompleted = true;
				resultException = e.Error;
			};

			retriever.LoadModuleType(fileModuleInfo);

			Assert.True(loadCompleted);
			Assert.NotNull(resultException);
		}

		[Fact]
		public void CanRetrieveWithCorrectRef()
		{
			var retriever = new ModuleTypeLoader();
			var moduleInfo = new DynamicModuleInfo { Ref = "file://somefile" };
			Assert.True(retriever.CanLoadModuleType(moduleInfo));
		}

		[Fact]
		public void CannotRetrieveWithIncorrectRef()
		{
			var retriever = new ModuleTypeLoader();
			var moduleInfo = new DynamicModuleInfo { Ref = "NotForLocalRetrieval" };

			Assert.False(retriever.CanLoadModuleType(moduleInfo));
		}

		[Fact]
		public void FileModuleTypeLoaderCanBeDisposed()
		{
			var typeLoader = new ModuleTypeLoader();
			Assert.IsAssignableFrom<IDisposable>(typeLoader );
		}

		[Fact]
		public void FileModuleTypeLoaderDisposeNukesAssemblyResolver()
		{
			Mock<IAssemblyResolver> mockResolver = new Mock<IAssemblyResolver>();
			var disposableMockResolver = mockResolver.As<IDisposable>();
			disposableMockResolver.Setup(resolver => resolver.Dispose());

			var typeLoader = new ModuleTypeLoader(mockResolver.Object);
			
			typeLoader.Dispose();

			disposableMockResolver.Verify(resolver => resolver.Dispose(), Times.Once());
		}

		[Fact]
		public void FileModuleTypeLoaderDisposeDoesNotThrowWithNonDisposableAssemblyResolver()
		{
			var mockResolver = new Mock<IAssemblyResolver>();
			var typeLoader = new ModuleTypeLoader(mockResolver.Object);
			typeLoader.Dispose();
		}

		static ModuleInfo CreateModuleInfo(string assemblyFile, string moduleType, string moduleName, bool startupLoaded, params string[] dependsOn)
		{
			var moduleInfo = new DynamicModuleInfo(moduleName, moduleType)
			{
				InitializationMode = startupLoaded ? InitializationMode.WhenAvailable : InitializationMode.OnDemand,
				Ref = assemblyFile,
			};
			if (dependsOn != null)
			{
				moduleInfo.DependsOn.AddRange(dependsOn);
			}

			return moduleInfo;
		}
	}

	class MockAssemblyResolver : IAssemblyResolver
	{
		public string LoadAssemblyFromArgument;
		public bool ThrowOnLoadAssemblyFrom;

		public void LoadAssemblyFrom(string assemblyFilePath)
		{
			LoadAssemblyFromArgument = assemblyFilePath;
			if (ThrowOnLoadAssemblyFrom)
				throw new Exception();
		}
	}
}
