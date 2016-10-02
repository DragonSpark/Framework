using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using Moq;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Markup
{
	public sealed class MockFactory : ParameterizedSourceBase<Type, object>
	{
		public static IParameterizedSource<Type, object> Default { get; } = new MockFactory().Apply( Specification.DefaultNested );
		MockFactory() : this( Activator.Default.Get<Mock> ) {}

		readonly Func<Type, Mock> activator;

		public MockFactory( Func<Type, Mock> activator )
		{
			this.activator = activator;
		}

		class Specification : SpecificationBase<Type>
		{
			public static Specification DefaultNested { get; } = new Specification();
			Specification() {}

			public override bool IsSatisfiedBy( Type parameter ) => parameter.IsInterface || !parameter.IsSealed;
		}

		public override object Get( Type parameter )
		{
			var type = typeof(Mock<>).MakeGenericType( parameter );
			var result = activator( type ).Object;
			return result;
		}
	}
}