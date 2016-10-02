using DragonSpark.Activation.Location;
using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.Extensions;
using Xunit;

namespace DragonSpark.Testing.Setup
{
	public class SetupTests
	{
		[Fact]
		public void ServiceProviderCaching()
		{
			var count = 0;
			Exports.Default.Assign( () =>
									 {
										++count;
										return DefaultExportProvider.Default;
									 }  );
			Assert.Equal( 0, count );

			var serviceProvider = DefaultServices.Default.Cached();
			serviceProvider.Get<IExportProvider>();
			Assert.Equal( 1, count );
			serviceProvider.Get<IExportProvider>();
			Assert.Equal( 1, count );

			var provider = ActivatorFactory.Default.Get();
			Assert.Equal( 2, count );

			provider.Get<IExportProvider>();
			Assert.Equal( 3, count );

			for ( var i = 0; i < 10; i++ )
			{
				provider.Get<IExportProvider>();
				Assert.Equal( 3, count );
			}
		}
	}
}