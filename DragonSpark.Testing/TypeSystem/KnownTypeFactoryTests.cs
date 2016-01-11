using System;
using System.Reflection;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class KnownTypeFactoryTests
	{
		[RegisterFactory( typeof(AssemblyProvider) )]
		[Theory, AutoDataRegistration]
		public void Testing( KnownTypeFactory sut )
		{
			var parameter = typeof(Class);

			var items = sut.Create( parameter );

			Assert.NotEmpty( items );

			Assert.All( items, type => Assert.True( type.IsSubclassOf( parameter ) ) );
		}
	}
}