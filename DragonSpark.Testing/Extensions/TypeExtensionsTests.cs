using DragonSpark.Extensions;
using DragonSpark.Testing.TestObjects;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class TypeExtensionsTests
	{
		[Fact]
		public void GetHierarchy()
		{
			Assert.Equal( new[]{ typeof(Derived), typeof(Class), typeof(object) }, typeof(Derived).Adapt().GetHierarchy().ToArray() );
			Assert.Equal( new[]{ typeof(Derived), typeof(Class) }, typeof(Derived).Adapt().GetHierarchy( false ).ToArray() );
		}

		[Fact]
		public void GetAllInterfaces()
		{
			var interfaces = typeof(Derived).Adapt().GetAllInterfaces().OrderBy( x => x.Name ).ToArray();
			Assert.Equal( new[]{ typeof(IAnotherInterface), typeof(IInterface) }, interfaces );
		}

		[Fact]
		public void GetItemType()
		{
			Assert.Equal( typeof(Class), typeof(List<Class>).Adapt().GetInnerType() );
			Assert.Equal( typeof(Class), typeof(Class[]).Adapt().GetInnerType() );
			Assert.Null( typeof(Class).Adapt().GetInnerType() );
		}
	}
}