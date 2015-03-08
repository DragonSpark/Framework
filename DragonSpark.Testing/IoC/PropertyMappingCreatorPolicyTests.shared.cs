using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using DragonSpark.IoC.Io;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityContainer = Microsoft.Practices.Unity.UnityContainer;

namespace DragonSpark.Testing.IoC
{
	/// <summary>
	/// Summary description for PropertyMappingCreatorPolicyTests
	/// </summary>
	[TestClass]
	public class PropertyMappingCreatorPolicyTests
	{
		[TestMethod]
		public void VerifyPolicyWorks()
		{
			var instance = new TargetObject
			               	{
			               		TargetProperty =
			               			new TargetObjectItem { Name = "Hello World" }
			               	};
			var container = new UnityContainer().AddNewExtension<DragonSparkExtension>().RegisterInstance( instance );
			var factory = new ExpressionFactory { Expression = "TargetProperty", Source = new NamedTypeBuildKey { BuildType = typeof(TargetObject)} }.Create( container, typeof(TargetObjectItem) );
			container.RegisterType( typeof(TargetObjectItem), factory );
			
			// factory.Configure( container, new UnityType { Type = typeof(TargetObjectItem) } );
			// container.Configure<IStaticFactoryConfiguration>().RegisterFactory<TargetObjectItem>(  )
			/*container.RegisterType<TargetObjectItem>( new PolicyInjection( typeof(IFactoryPolicy),
			                                                               new ExpressionFactory
			                                                               	{
			                                                               		Expression = "TargetProperty",
			                                                               		TargetBuildType = typeof(TargetObject)
			                                                               	} ) );*/
			var target = container.Resolve<TargetObjectItem>();
			Assert.IsNotNull( target );
			Assert.AreEqual( "Hello World", target.Name );
			Assert.AreSame( instance.TargetProperty, target );
		}
	}
}
