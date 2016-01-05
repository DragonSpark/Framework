using System;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Setup;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using SetupParameter = DragonSpark.Setup.SetupParameter;

namespace DragonSpark.Windows.Testing.Setup
{
	public class SetupParameterTests
	{
		[Theory, DragonSpark.Testing.Framework.Setup.AutoData]
		public void Constructor( int number, Task task, IDisposable disposable )
		{
			var parameter = new SetupParameter();
			using ( parameter )
			{
				parameter.Register( Tuple.Create( number ) );
				Assert.Null( parameter.Arguments );
				Assert.Equal( number, parameter.Item<Tuple<int>>().Item1 );
				Assert.Same( disposable,  parameter.RegisterFor( disposable ) );
				Assert.Same( task, parameter.Monitor<Task>( task ) );
			}
			Mock.Get( disposable ).Verify( d => d.Dispose() );
			Assert.True( task.IsCompleted );
			Assert.False( parameter.Items.Any() );
		}

		[Theory, DragonSpark.Testing.Framework.Setup.AutoData]
		public void New( [Frozen]int number, [Greedy]SetupParameter<int> sut )
		{
			Assert.Equal( number, sut.Arguments );
		}
	}
}