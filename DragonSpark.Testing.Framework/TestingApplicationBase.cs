
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public class TestingContext : ServiceLocatorFactory
	{
		public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
		{
			get { return Framework.TestContext.Current; }
			set { Framework.TestContext.Assign( value ); }
		}

		/*protected override IModuleCatalog CreateModuleCatalog()
		{
			return new DirectoryModuleCatalog { ModulePath = @"." };
		}*/

		protected IUnityContainer Container { get; private set; }
		protected IServiceLocator Locator { get; private set; }

		[TestInitialize]
		public void InitializeTest()
		{
			Locator = Create();
			Container = Locator.GetInstance<IUnityContainer>();

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
			var context = new TestMethodProcessingContext( Locator, TestContext, method );
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

			Locator.TryDispose();

			OnTestCleanUp( EventArgs.Empty );
		}
	}
}