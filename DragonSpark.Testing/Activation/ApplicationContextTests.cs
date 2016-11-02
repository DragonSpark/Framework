using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.Commands;
using DragonSpark.Configuration;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Framework.Runtime;
using JetBrains.Annotations;
using System.Composition;
using System.Reflection;
using Xunit;
using ExecutionContext = DragonSpark.Testing.Framework.Runtime.ExecutionContext;

namespace DragonSpark.Testing.Activation
{
	[ContainingTypeAndNested]
	public class ApplicationContextTests
	{
		[Fact]
		public void Assignment()
		{
			var before = Execution.Default.GetValue();
			var current = Assert.IsType<TaskContext>( before );
			Assert.Same( ExecutionContext.Default.Get(), current );
			Assert.Equal( Identification.Default.Get(), current.Origin );
			Assert.Null( CurrentMethod.Default.Get() );
			Assert.True( EnableMethodCaching.Default.Get() );

			var method = MethodBase.GetCurrentMethod();
			object inner;
			using ( ApplicationFactory.Default.Get( method ).Run( new AutoData( CurrentFixture.Default.WithFactory( FixtureFactory<DefaultAutoDataCustomization>.Default.Get ), method ) ) )
			{
				Assert.NotNull( CurrentMethod.Default.Get() );
				Assert.Same( method, CurrentMethod.Default.Get() );
				inner = Execution.Default.GetValue();
				Assert.Same( current, inner );
				var condition = EnableMethodCaching.Default.Get();
				Assert.False( condition );
			}

			var after = Execution.Default.GetValue();
			Assert.NotNull( after );
			Assert.NotSame( inner, after );
			Assert.NotSame( current, after );

			Assert.Null( CurrentMethod.Default.Get() );

			Assert.True( EnableMethodCaching.Default.Get() );
		}

		[Export( typeof(ISetup) ), UsedImplicitly]
		class Setup : DeclarativeSetup
		{
			public Setup() : base( EnableMethodCaching.Default.ToCommand( false ) ) {}
		}
	}
}