using System.ServiceModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DragonSpark.Testing.Communications.Configuration
{
	/// <summary>
	/// Interaction logic for ServiceLocatorInstanceElementTests.xaml
	/// </summary>
	[TestClass]
	public partial class ServiceLocatorInstanceElementTests
	{
		public ServiceLocatorInstanceElementTests()
		{
			InitializeComponent();
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void EnsureSubjectIsNotNull()
		{
			Assert.IsNotNull( Subject );

			Subject.Open();

			var serviceEndpoint = Subject.Description.Endpoints.First();
			var client = new ChannelFactory<ITestingService>( serviceEndpoint.Binding, serviceEndpoint.Address ).CreateChannel();
			var result = client.HelloWorld( "Hello!" );
			Assert.AreEqual( "This is a baseline message: Hello!", result );
		}
	}
}
