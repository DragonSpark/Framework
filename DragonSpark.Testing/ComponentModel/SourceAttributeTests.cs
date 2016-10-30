using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	public class SourceAttributeTests
	{
		[Fact]
		public void GetResultType()
		{
			var expected = typeof(FactoryOfYac).Yield().AsApplicationParts();
			var type = SourceTypes.Default.Get( typeof(YetAnotherClass) );
			Assert.Equal( expected.Single(), type );
		}

		[Fact]
		public void Property()
		{
			Assert.IsType<YetAnotherClass>( ClassProperty );
		}

		[Fact]
		public void PropertyFromApplicationTypes()
		{
			typeof(FactoryOfYac).Yield().AsApplicationParts();
			Assert.IsType<YetAnotherClass>( ClassPropertyFromApplicationTypes );
		}

		[Source( typeof(FactoryOfYac) )]
		public IInterface ClassProperty { get; set; }

		[Source]
		public YetAnotherClass ClassPropertyFromApplicationTypes { get; set; }
	}

	
}