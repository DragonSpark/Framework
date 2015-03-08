
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Framework
{
	public class TestingContext : Launcher
	{
		public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
		{
			get { return Framework.TestContext.Current; }
			set { Framework.TestContext.Assign( value ); }
		}

		protected override IModuleCatalog CreateModuleCatalog()
		{
			return new DirectoryModuleCatalog { ModulePath = @"." };
		}

		[TestInitialize]
		public void InitializeTest()
		{
			Run();

			OnTestInitializing( EventArgs.Empty );

			PrepareTest();

			InitializeSubject();

			OnTestInitialized( EventArgs.Empty );
		}

		protected virtual void InitializeSubject()
		{
			Container.BuildUp( GetType(), this );
		}

		protected virtual void PrepareTest()
		{
			var method = ResolveCurrentMethod();
			var context = new TestMethodProcessingContext( ServiceLocator, TestContext, method );
			Attributes.Apply( item => item.Process( context ) );
		}

		IEnumerable<TestMethodProcessorAttribute> Attributes
		{
			get { return attributes ?? ( attributes = ResolveAttributes() ); }
		}	TestMethodProcessorAttribute[] attributes;

		TestMethodProcessorAttribute[] ResolveAttributes()
		{
			var method = ResolveCurrentMethod();
			var result = method.Transform( m => m.GetAttributes<TestMethodProcessorAttribute>().OrderByDescending( x => x.Priority ).ToArray() );
			return result;
		}

		protected virtual void OnTestInitializing( EventArgs args )
		{}

		protected virtual void OnTestInitialized( EventArgs args )
		{}

		protected virtual void OnTestCleaningUp( EventArgs args )
		{
			Container.Teardown( this );
		}

		protected virtual void OnTestCleanUp( EventArgs args )
		{}

		protected MethodInfo ResolveCurrentMethod()
		{
			return TestContext.Transform( item => GetType().GetMethod( item.TestName ) );
		}


		[TestCleanup]
		public void CleanupTest()
		{
			OnTestCleaningUp( EventArgs.Empty );

			Attributes.OfType<IDisposable>().Apply( x => x.Dispose() );

			ServiceLocator.Dispose();

			OnTestCleanUp( EventArgs.Empty );
		}

		protected override DependencyObject CreateShell()
		{
			return null;
		}
	}
}