using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Setup;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Setup
{
	public class SetupParameterTests
	{
		[Theory, MoqAutoData]
		public void Constructor( int number, Task task, IDisposable disposable )
		{
			var parameter = new SetupParameter<object>();
			using ( parameter )
			{
				parameter.Register( Tuple.Create( number ) );
				Assert.Null( parameter.GetArguments() );
				Assert.Equal( number, parameter.Item<Tuple<int>>().Item1 );
				Assert.Same( disposable,  parameter.RegisterFor( disposable ) );
				Assert.Same( task, parameter.Monitor<Task>( task ) );
			}
			Mock.Get( disposable ).Verify( d => d.Dispose() );
			Assert.True( task.IsCompleted );
			Assert.False( parameter.Items.Any() );
		} 
	}
}