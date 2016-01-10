using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	public class ProgramSetupTests
	{
		[Theory, ProgramSetup.AutoData]
		public void Extension( [Located] SetupParameter sut )
		{
			var collection = new Items( sut ).Item;
			var module = collection.FirstOrDefaultOfType<MonitoredModule>();
			Assert.NotNull( module );
			Assert.True( module.Initialized );
			Assert.True( module.Loaded );

			var command = collection.FirstOrDefaultOfType<MonitoredModule.Command>();
			Assert.NotNull( command );
		}

		[Theory, ProgramSetup.AutoData]
		public void Create( [Located]ApplicationInformation sut, AttributeProvider provider )
		{
			Assert.NotNull( sut.AssemblyInformation );
			Assert.Equal( DateTimeOffset.Parse( "2/1/2016" ), sut.DeploymentDate.GetValueOrDefault() );
			Assert.Equal( "http://framework.dragonspark.us/testing", sut.CompanyUri.ToString() );
			var assembly = GetType().Assembly;
			Assert.Equal( provider.FromMetadata<AssemblyTitleAttribute, string>( assembly, attribute => attribute.Title ), sut.AssemblyInformation.Title );
			Assert.Equal( provider.FromMetadata<AssemblyCompanyAttribute, string>( assembly, attribute => attribute.Company ), sut.AssemblyInformation.Company );
			Assert.Equal( provider.FromMetadata<AssemblyCopyrightAttribute, string>( assembly, attribute => attribute.Copyright ), sut.AssemblyInformation.Copyright );
			Assert.Equal( provider.FromMetadata<DebuggableAttribute, string>( assembly, attribute => "DEBUG" ), sut.AssemblyInformation.Configuration );
			Assert.Equal( provider.FromMetadata<AssemblyDescriptionAttribute, string>( assembly, attribute => attribute.Description ), sut.AssemblyInformation.Description );
			Assert.Equal( provider.FromMetadata<AssemblyProductAttribute, string>( assembly, attribute => attribute.Product ), sut.AssemblyInformation.Product );
			Assert.Equal( assembly.GetName().Version, sut.AssemblyInformation.Version );
		}

		[Theory, ProgramSetup.AutoData]
		public void Type( IUnityContainer sut )
		{
			var resolve = sut.Resolve<ITyper>();
			Assert.IsType<SomeTypeist>( resolve );
		}

		[Theory, ProgramSetup.AutoData]
		public void Run( [Located]Program sut )
		{
			Assert.True( sut.Ran, "Didn't Run" );
			Assert.Equal( GetType().GetMethod( nameof(Run) ), sut.Arguments.Method );
		}

		[Theory, ProgramSetup.AutoData]
		public void SetupModuleCommand( [Located] SetupParameter parameter, [Located] SetupModuleCommand sut, [Located] MonitoredModule module )
		{
			var added = new Items( module ).Item.FirstOrDefaultOfType<SomeCommand>();
			Assert.Null( added );
			sut.Execute( module );

			Assert.NotNull( new Items( module ).Item.FirstOrDefaultOfType<SomeCommand>() );
		}
	}

	public class Program : Program<AutoData>
	{
		public bool Ran { get; private set; }

		public AutoData Arguments { get; private set; }

		protected override void Run( AutoData arguments )
		{
			Ran = true;
			Arguments = arguments;
		}
	}

	public class SomeTypeist : ITyper
	{ }
	public interface ITyper
	{}

	public class SomeCommand : ModuleCommand
	{
		protected override void OnExecute( IMonitoredModule parameter ) => new Items( parameter ).Item.Add( this );
	}

	public class MonitoredModule : MonitoredModule<MonitoredModule.Command>
	{
		public MonitoredModule( IModuleMonitor moduleMonitor, ISetupParameter parameter, Command command ) : base( moduleMonitor, command )
		{
			new Items( parameter ).Item.Add( this );
		}

		public bool Initialized { get; private set; }

		public bool Loaded { get; private set; }
		

		protected override void OnInitialize()
		{
			Initialized = true;
			base.OnInitialize();
		}

		protected override void OnLoad()
		{
			Loaded = true;
			base.OnLoad();
		}

		public class Command : ModuleCommand
		{
			readonly ISetupParameter setup;

			public Command( ISetupParameter setup )
			{
				this.setup = setup;
			}

			protected override void OnExecute( IMonitoredModule parameter ) => new Items( setup ).Item.Add( this );
		}
	}
}