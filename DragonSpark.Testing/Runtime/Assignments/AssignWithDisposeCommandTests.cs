using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources;
using Xunit;

namespace DragonSpark.Testing.Runtime.Assignments
{
	public class AssignWithDisposeCommandTests
	{
		[Fact]
		public void Assign()
		{
			var source = new SuppliedSource<object>();
			var item = new object();
			source.Assign( item );
			using ( var command = new AssignWithDisposeCommand<object>( source ) )
			{
				Assert.Same( item, source.Get() );
				var assign = new object();
				command.Execute( assign );
				Assert.Same( assign, source.Get() );
			}
			Assert.Null( source.Get() );
		}

		[Fact]
		public void Restore()
		{
			var source = new SuppliedSource<object>();
			var item = new object();
			source.Assign( item );
			using ( var command = new AssignWithRestoreCommand<object>( source, item ) )
			{
				Assert.Same( item, source.Get() );
				var assign = new object();
				command.Execute( assign );
				Assert.Same( assign, source.Get() );
			}
			Assert.Same( item, source.Get() );
		}
	}
}