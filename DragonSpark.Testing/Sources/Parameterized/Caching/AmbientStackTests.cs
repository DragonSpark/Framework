using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Testing.Objects;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class AmbientStackTests
	{
		readonly Stacks<Class> cache = new Stacks<Class>();

		[Fact]
		public void GetCurrentItem()
		{
			var item = new object();
			AmbientStack<object>.Default.Get().Push( item );

			Assert.Same( item, AmbientStack.GetCurrentItem( typeof(object) ) );
		}

		[Fact]
		public void ContextAsExpected()
		{
			var stack = new AmbientStack<Class>( cache );

			var expected = stack.Get();
			Assert.Same( expected, stack.Get() );

			Assert.Null( stack.GetCurrentItem() );

			var first = new Class();
			using ( new AmbientStackCommand<Class>( stack ).Run( first ) )
			{
				Assert.Same( first, stack.GetCurrentItem() );

				var second = new Class();
				using ( new AmbientStackCommand<Class>( stack ).Run( second ) )
				{
					Assert.Same( second, stack.GetCurrentItem() );

					var third = new Class();
					using ( new AmbientStackCommand<Class>( stack ).Run( third ) )
					{
						Assert.Same( third, stack.GetCurrentItem() );

						var chain = stack.Get().All();
						Assert.Equal( 3, chain.Length );
						Assert.Same( chain.First(), third );
						Assert.Same( chain.Last(), first );

						var inside = new Class();
						var appended = inside.Append( stack.Get().All().ToArray() ).ToArray();
						Assert.Equal( 4, appended.Length );
						Assert.Same( appended.First(), inside );
						Assert.Same( appended.Last(), first );

						Task.Run( () =>
						{
							var thread = stack.Get();
							Assert.Same( thread, stack.Get() );
							Assert.NotSame( expected, thread );
							Assert.Empty( stack.Get().All().ToArray() );
							var other = new Class();
							using ( new AmbientStackCommand<Class>( stack ).Run( other ) )
							{
								Assert.Same( other, stack.GetCurrentItem() );
								Assert.Single( stack.Get().All().ToArray(), other );
							}
							Assert.Same( thread, stack.Get() );
						} ).Wait();
					}

					Assert.Same( second, stack.GetCurrentItem() );
				}

				Assert.Single( stack.Get().All().ToArray(), first );
				Assert.Same( first, stack.GetCurrentItem() );
			}

			Assert.Null( stack.GetCurrentItem() );

			Assert.NotSame( expected, stack.Get() );
		} 
	}
}