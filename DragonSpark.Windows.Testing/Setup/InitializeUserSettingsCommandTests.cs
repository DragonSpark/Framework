using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using DragonSpark.Windows.Properties;
using DragonSpark.Windows.Setup;
using JetBrains.Annotations;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Configuration;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Windows.Testing.Setup
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	public class InitializeUserSettingsCommandTests : TestCollectionBase
	{
		public InitializeUserSettingsCommandTests( ITestOutputHelper output ) : base( output )
		{
			// SaveUserSettingsCommand.Default.Configuration.Assign( () => p => p.Save() );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, InitializeUserSettingsFile]
		public void VerifyFileOne( IFile file, UserSettingsFilePath sut, IDirectorySource source )
		{
			var path = sut.Get();
			Assert.StartsWith( source.Get(), path );
			Assert.True( file.Exists( path ) );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void Create( 
			[NoAutoProperties]ApplicationSettingsBase parameter, 
			[DragonSpark.Testing.Framework.Application.Service]InitializeUserSettingsCommand sut, 
			ILoggerHistory history, 
			[DragonSpark.Testing.Framework.Application.Service]UserSettingsFile factory, 
			ClearUserSettingCommand clear )
		{
			var path = factory.Get();
			// parameter.Setup( p => p.Save() ).Callback( () => RegisterFilesCommand.Default.Execute( path.FullName ) ).Verifiable();

			Assert.NotNull( path );
			Assert.False( path.Exists, path.FullName );
			var before = history.Events.Fixed();

			sut.Execute( parameter );

			/*parameter.Verify( p => p.Upgrade(), Times.Once );
			parameter.Verify( p => p.Save(), Times.Once );*/

			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			Assert.Contains( Resources.LoggerTemplates_NotFound, items );
			Assert.Contains( Resources.LoggerTemplates_Created, items );
			Assert.Equal( before.Length + 3, items.Length );

			Assert.True( path.Refreshed().Exists );

			clear.Execute();
			history.Clear();

			Assert.Empty( history.Events );

			Assert.False( path.Refreshed().Exists );
			sut.Execute( parameter );
			Assert.True( path.Refreshed().Exists );
			
			Assert.Equal( 3, history.Events.Count() );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void CreateThenRecreate( [NoAutoProperties]Mock<ApplicationSettingsBase> parameter, [DragonSpark.Testing.Framework.Application.Service]InitializeUserSettingsCommand sut, ILoggerHistory history )
		{
			parameter.Setup( p => p.Save() ).Verifiable();

			sut.Execute( parameter.Object );
			
			var created = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			Assert.Contains( Resources.LoggerTemplates_NotFound, created );
			Assert.Contains( Resources.LoggerTemplates_Created, created );

			var count = history.Events.Count();

			sut.Execute( parameter.Object );

			Assert.Equal( count, history.Events.Count() );

			parameter.Verify( p => p.Upgrade(), Times.Once );
			parameter.Verify( p => p.Save(), Times.Once );
			parameter.VerifyAll();
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void NoProperties( [DragonSpark.Testing.Framework.Application.Service]InitializeUserSettingsCommand sut, ILoggerHistory history )
		{
			var before = history.Events.Fixed();
			sut.Execute( new SettingsWithNoProperties() );
			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			var expected = new[] { Resources.LoggerTemplates_Initializing, Resources.LoggerTemplates_NotFound, Resources.LoggerTemplates_Created };
			foreach ( var message in expected )
			{
				Assert.Contains( message, items );
			}
			Assert.Equal( before.Length + expected.Length, items.Length );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void UpgradeWithFileCreate( [DragonSpark.Testing.Framework.Application.Service]InitializeUserSettingsCommand sut, ILoggerHistory history )
		{
			var before = history.Events.Fixed();
			sut.Execute( new UpgradeWithCompleteSettings() );
			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			var expected = new[] { Resources.LoggerTemplates_Initializing, Resources.LoggerTemplates_Complete };
			foreach ( var message in expected )
			{
				Assert.Contains( message, items );
			}
			Assert.Equal( before.Length + expected.Length, items.Length );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void Error( [DragonSpark.Testing.Framework.Application.Service]InitializeUserSettingsCommand sut, ILoggerHistory history )
		{
			var before = history.Events.Fixed();
			sut.Execute( new SettingsWithException() );
			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			var expected = new[] { Resources.LoggerTemplates_Initializing, Resources.LoggerTemplates_NotFound, Resources.LoggerTemplates_ErrorSaving };
			foreach ( var message in expected )
			{
				Assert.Contains( message, items );
			}
			Assert.Equal( before.Length + expected.Length, items.Length );
		}

		class Settings : ApplicationSettingsBase
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

		class SettingsWithNoProperties : ApplicationSettingsBase {}

		class SettingsWithException : ApplicationSettingsBase
		{
			[UserScopedSetting, UsedImplicitly]
			public string HelloWorld
			{
				get { return (string)this[nameof(HelloWorld)]; }
				set { this[nameof(HelloWorld)] = value; }
			}

			public override void Save()
			{
				throw new ConfigurationErrorsException( "Some exception" );
			}
		}

		sealed class UpgradeWithCompleteSettings : ApplicationSettingsBase
		{
			public override void Upgrade()
			{
				SaveUserSettingsCommand.Default.Execute( this );
				base.Upgrade();
			}
		}
	}
}