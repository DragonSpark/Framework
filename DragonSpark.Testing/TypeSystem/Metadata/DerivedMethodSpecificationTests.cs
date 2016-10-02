using DragonSpark.TypeSystem.Metadata;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.TypeSystem.Metadata
{
	public class DerivedMethodSpecificationTests
	{
		[Theory, AutoData]
		public void Properties( DerivedMethodSpecification sut )
		{
			Assert.True( sut.IsSatisfiedBy( typeof(Class).GetProperty( nameof(Class.Override) ).GetMethod ) );
			Assert.False( sut.IsSatisfiedBy( typeof(Class).GetProperty( nameof(Class.Virtual) ).GetMethod ) );
			Assert.False( sut.IsSatisfiedBy( typeof(BaseClass).GetProperty( nameof(Class.Override) ).GetMethod ) );
			
		}

		[Theory, AutoData]
		public void Methods( DerivedMethodSpecification sut )
		{
			Assert.True( sut.IsSatisfiedBy( typeof(Class).GetMethod( nameof(Class.Method) ) ) );
			Assert.False( sut.IsSatisfiedBy( typeof(BaseClass).GetMethod( nameof(Class.Method) ) ) );
		}

		class BaseClass
		{
			public virtual void Method() {}

			public virtual string Virtual { get; set; }

			public virtual string Override { get; set; }
		}

		class Class : BaseClass
		{
			public override void Method() {}

			public override string Override
			{
				get { return base.Override; }
				set { }
			}
		}
	}
}