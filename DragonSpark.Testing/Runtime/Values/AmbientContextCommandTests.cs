using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Runtime.Values
{
	public class AmbientContextCommandTests
	{
		[Fact]
		public void ContextAsExpected()
		{
			var stack = new ThreadAmbientValue<Class>().Item;
			Assert.Same( stack, new ThreadAmbientValue<Class>().Item );

			Assert.Null( Ambient.GetCurrent<Class>() );

			var first = new Class();
			using ( new AmbientContextCommand<Class>().ExecuteWith( first ) )
			{
				Assert.Same( first, Ambient.GetCurrent<Class>() );

				var second = new Class();
				using ( new AmbientContextCommand<Class>().ExecuteWith( second ) )
				{
					Assert.Same( second, Ambient.GetCurrent<Class>() );

					var third = new Class();
					using ( new AmbientContextCommand<Class>().ExecuteWith( third ) )
					{
						Assert.Same( third, Ambient.GetCurrent<Class>() );

						var chain = Ambient.GetCurrentChain<Class>();
						Assert.Equal( 3, chain.Length );
						Assert.Same( chain.First(), third );
						Assert.Same( chain.Last(), first );

						var inside = new Class();
						var appended = inside.GetCurrentChain();
						Assert.Equal( 4, appended.Length );
						Assert.Same( appended.First(), inside );
						Assert.Same( appended.Last(), first );

						Task.Run( () =>
						{
							var thread = new ThreadAmbientValue<Class>().Item;
							Assert.NotSame( stack, thread );
							Assert.Empty( thread );
							var other = new Class();
							using ( new AmbientContextCommand<Class>().ExecuteWith( other ) )
							{
								Assert.Same( other, Ambient.GetCurrent<Class>() );
								Assert.Single( thread, other );
							}
							Assert.NotSame( thread, new ThreadAmbientValue<Class>().Item );
						} ).Wait();
					}

					Assert.Same( second, Ambient.GetCurrent<Class>() );
				}

				Assert.Single( Ambient.GetCurrentChain<Class>(), first );
				Assert.Same( first, Ambient.GetCurrent<Class>() );
			}

			Assert.Null( Ambient.GetCurrent<Class>() );

			Assert.NotSame( stack, new ThreadAmbientValue<Class>().Item );
		} 
	}
}