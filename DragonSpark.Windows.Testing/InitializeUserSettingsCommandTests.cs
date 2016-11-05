using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using DragonSpark.Windows.Properties;
using JetBrains.Annotations;
using Moq;
using System;
using System.Configuration;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Windows.Testing
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	public class InitializeUserSettingsCommandTests : TestCollectionBase
	{
		public InitializeUserSettingsCommandTests( ITestOutputHelper output ) : base( output )
		{
			SaveUserSettingsCommand.Default.Configuration.Assign( () => p => p.Save() );
		}

		[Theory, AutoData, InitializeUserSettingsFile]
		public void VerifyFileOne( [Service]IFile file, UserSettingsFilePath sut, [Service]IDirectorySource source )
		{
			var path = sut.Get();
			Assert.StartsWith( source.Get(), path );
			Assert.True( file.Exists( path ) );
		}

		[Theory, AutoData]
		void Create( 
			Mock<Settings> parameter, 
			InitializeUserSettingsCommand sut, 
			[Service]ILoggerHistory history, 
			UserSettingsFile factory, 
			ClearUserSettingCommand clear,
			UserSettingsExistsSpecification specification, [Service]IFileSystemRepository repository )
		{
			var path = factory.Get();
			
			Assert.NotNull( path );
			Assert.False( path.Exists, path.FullName );
			Assert.False( specification.IsSatisfied() );
			var before = history.Events.Fixed();

			sut.Execute( parameter.Object );

			parameter.Verify( p => p.Upgrade(), Times.Once );
			parameter.Verify( p => p.Save(), Times.Once );

			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			Assert.Contains( Resources.LoggerTemplates_NotFound, items );
			Assert.Contains( Resources.LoggerTemplates_Created, items );
			Assert.Equal( before.Length + 3, items.Length );

			Assert.True( specification.IsSatisfied() );
			Assert.True( path.Refreshed().Exists );

			clear.Execute();
			history.Clear();

			Assert.Empty( history.Events );

			Assert.False( path.Refreshed().Exists );
			sut.Execute( parameter.Object );
			Assert.True( path.Refreshed().Exists );
			
			Assert.Equal( 3, history.Events.Count() );
		}

		[Theory, AutoData]
		public void CreateThenRecreate( 
			Mock<Settings> parameter, 
			InitializeUserSettingsCommand sut, 
			[Service]ILoggerHistory history, 
			UserSettingsExistsSpecification specification )
		{
			Assert.False( specification.IsSatisfied() );
		
			sut.Execute( parameter.Object );
			Assert.True( specification.IsSatisfied() );
			
			var created = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			Assert.Contains( Resources.LoggerTemplates_NotFound, created );
			Assert.Contains( Resources.LoggerTemplates_Created, created );

			var count = history.Events.Count();

			sut.Execute( parameter.Object );

			Assert.Equal( count, history.Events.Count() );

			parameter.Verify( p => p.Upgrade(), Times.Once );
			parameter.Verify( p => p.Save(), Times.Once );
		}

		[Theory, AutoData]
		public void UpgradeWithFileCreate( Mock<Settings> parameter, InitializeUserSettingsCommand sut, [Service]ILoggerHistory history )
		{
			parameter.Setup( settings => settings.Upgrade() ).Callback( () => parameter.Object.Save() );

			var before = history.Events.Fixed();
			sut.Execute( parameter.Object );
			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			var expected = new[] { Resources.LoggerTemplates_Initializing, Resources.LoggerTemplates_Complete };
			foreach ( var message in expected )
			{
				Assert.Contains( message, items );
			}
			Assert.Equal( before.Length + expected.Length, items.Length );
		}

		[Theory, AutoData]
		public void Error( Mock<ApplicationSettingsBase> parameter, InitializeUserSettingsCommand sut, [Service]ILoggerHistory history, [Service]IFileSystemRepository repository )
		{
			parameter.Setup( settings => settings.Save() ).Throws( new ConfigurationErrorsException( "Some exception" ) );

			var before = history.Events.Fixed();
			sut.Execute( parameter.Object );
			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			var expected = new[] { Resources.LoggerTemplates_Initializing, Resources.LoggerTemplates_NotFound, Resources.LoggerTemplates_ErrorSaving };
			foreach ( var message in expected )
			{
				Assert.Contains( message, items );
			}
			Assert.Equal( before.Length + expected.Length, items.Length );
		}

		

		[UsedImplicitly]
		public class Settings : ApplicationSettingsBase
		{
			readonly Action save;

			public Settings() : this( Register.Default.Execute ) {}

			public Settings( Action save )
			{
				this.save = save;
			}

			public override void Save() => save();

			sealed class Register : SuppliedCommand<string>
			{
				public static Register Default { get; } = new Register();
				Register() : base( RegisterFileCommand.Default, UserSettingsFilePath.Default.ToDelegate() ) {}
			}
		}
	}
}