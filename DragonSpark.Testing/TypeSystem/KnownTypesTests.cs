using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	public class KnownTypesTests
	{
		[Theory, AutoData, IncludeParameterTypes( typeof(Class), typeof(ClassWithProperty), typeof(Derived) )]
		public void Testing( KnownTypes sut )
		{
			var parameter = typeof(Class);

			var items = sut.Get( parameter );

			Assert.NotEmpty( items );

			Assert.All( items, item => Assert.True( parameter.IsAssignableFrom( item ) ) );
		}
	}
}