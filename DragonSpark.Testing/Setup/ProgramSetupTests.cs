using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.Setup
{
	[AssignExecution]
	public class ProgramSetupTests
	{
		[Theory, Test, SetupAutoData( typeof(ProgramSetup) )]
		public void Extension( [Located] SetupParameter sut )
		{
			var collection = new Items( sut ).Item;
			var module = collection.FirstOrDefaultOfType<Module>();
			Assert.NotNull( module );
			Assert.True( module.Initialized );
			Assert.True( module.Loaded );

			var command = collection.FirstOrDefaultOfType<Module.Command>();
			Assert.NotNull( command );
		}

		[Theory, Test, SetupAutoData( typeof(ProgramSetup) )]
		public void Type( IUnityContainer sut )
		{
			Assert.IsType<SomeTypeist>( sut.Resolve<ITyper>() );
		}

		[Theory, Test, SetupAutoData( typeof(ProgramSetup) )]
		public void Run( [Located]Program sut )
		{
			Assert.True( sut.Ran, "Didn't Run" );
			Assert.Equal( GetType().GetMethod( nameof(Run) ), sut.Arguments.Method );
		}
	}

	public class Program : Program<SetupAutoDataParameter>
	{
		public bool Ran { get; private set; }

		public SetupAutoDataParameter Arguments { get; private set; }

		protected override void Run( SetupAutoDataParameter arguments )
		{
			Ran = true;
			Arguments = arguments;
		}
	}

	public class SomeTypeist : ITyper
	{ }
	public interface ITyper
	{}

	public class Module : Module<Module.Command>
	{
		public Module( IActivator activator, IModuleMonitor moduleMonitor, SetupParameter parameter ) : base( activator, moduleMonitor, parameter )
		{
			new Items( parameter ).Item.Add( this );
		}

		public bool Initialized { get; private set; }

		public bool Loaded { get; private set; }
		

		protected override void Initialize()
		{
			Initialized = true;
			base.Initialize();
		}

		protected override void Load()
		{
			Loaded = true;
			base.Load();
		}

		public class Command : SetupCommand
		{
			protected override void Execute( ISetupParameter parameter )
			{
				new Items( parameter ).Item.Add( this );
			}
		}
	}
}