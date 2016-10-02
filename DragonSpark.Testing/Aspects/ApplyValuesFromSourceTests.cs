using DragonSpark.Activation.Location;
using DragonSpark.Application.Setup;
using DragonSpark.Aspects;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class ApplyValuesFromSourceTests
	{
		[Fact]
		public void Verify()
		{
			var source = new SourceItem { SourceProperty = 6776 };
			var repository = GlobalServiceProvider.GetService<IServiceRepository>();
			repository.Add( source );

			var copy = new SourceItem();
			Assert.Equal( source.SourceProperty, copy.SourceProperty );
			Assert.NotEqual( 0, copy.SourceProperty );
		}

		[ApplyValuesFromSource]
		class SourceItem
		{
			public int SourceProperty { get; set; }
		}
	}
}
