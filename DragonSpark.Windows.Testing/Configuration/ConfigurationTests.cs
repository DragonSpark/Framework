using DragonSpark.ComponentModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Objects.Properties;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Windows.Testing.Configuration
{
	[Trait( Traits.Category, Traits.Categories.Xaml ), FrameworkTypes, FormatterTypes, AdditionalTypes( typeof(ExpressionEvaluator) )]
	public class ConfigurationTests
	{
		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void FromConfiguration( DragonSpark.Testing.Objects.Declarative.Configuration.Values sut )
		{
			var settings = Settings.Default;
			var primary = sut.Get( "PrimaryKey" );
			Assert.Equal( settings.HelloWorld, primary );

			var alias = sut.Get( "Some Key" );
			Assert.Equal( settings.HelloWorld, alias );

			var notfound = sut.Get( "NotFound" );
			Assert.Null( notfound );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, AdditionalTypes( typeof(DragonSpark.Testing.Objects.Declarative.Configuration.Values) )]
		public void FromItem( [NoAutoProperties]DragonSpark.Testing.Objects.Declarative.Configuration.Item sut )
		{
			Assert.Equal( "This is a value from a MemberInfoKey", sut.SomeTestingProperty );
		}
	}
}