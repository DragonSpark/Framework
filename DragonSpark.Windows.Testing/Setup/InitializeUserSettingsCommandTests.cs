using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Objects.Properties;
using DragonSpark.Windows.Setup;
using JetBrains.Annotations;
using System.Configuration;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Resources = DragonSpark.Windows.Properties.Resources;

namespace DragonSpark.Windows.Testing.Setup
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	public class InitializeUserSettingsCommandTests : TestCollectionBase
	{
		public InitializeUserSettingsCommandTests( ITestOutputHelper output ) : base( output )
		{
			Clear();
		}

		static void Clear() => ClearUserSettingCommand.Default.Execute();

		[Theory, AutoData]
		public void Create( InitializeUserSettingsCommand sut, ILoggerHistory history, UserSettingsFile factory )
		{
			var path = factory.Get( ConfigurationUserLevel.PerUserRoamingAndLocal );
			Assert.False( path.Exists, path.FullName );
			var before = history.Events.Fixed();
			sut.Execute( Settings.Default );
			var items = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			Assert.Contains( Resources.LoggerTemplates_NotFound, items );
			Assert.Contains( Resources.LoggerTemplates_Created, items );
			Assert.Equal( before.Length + 3, items.Length );

			Assert.True( path.Refreshed().Exists );

			Clear();

			Assert.False( path.Refreshed().Exists );
			sut.Execute( Settings.Default );
			Assert.True( path.Refreshed().Exists );
			
			Assert.Equal( items.Length + 3, history.Events.Count() );
		}

		[Theory, AutoData]
		public void CreateThenRecreate( InitializeUserSettingsCommand sut, ILoggerHistory history )
		{
			sut.Execute( Settings.Default );
			var created = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			Assert.Contains( Resources.LoggerTemplates_NotFound, created );
			Assert.Contains( Resources.LoggerTemplates_Created, created );

			var count = history.Events.Count();

			sut.Execute( Settings.Default );

			Assert.Equal( count, history.Events.Count() );
			/*var upgraded = history.Events.Select( item => item.MessageTemplate.Text ).Fixed();
			Assert.Contains( Resources.LoggerTemplates_Initializing, upgraded );
			Assert.Contains( Resources.LoggerTemplates_Complete, upgraded );*/
		}

		[Theory, AutoData]
		public void NoProperties( InitializeUserSettingsCommand create, InitializeUserSettingsCommand sut, ILoggerHistory history )
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

		[Theory, AutoData]
		public void Error( InitializeUserSettingsCommand sut, ILoggerHistory history )
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
	}
}