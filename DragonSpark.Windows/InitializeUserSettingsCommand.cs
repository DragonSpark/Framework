using System;
using System.Configuration;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Windows.FileSystem;
using DragonSpark.Windows.Properties;
using JetBrains.Annotations;
using Serilog;

namespace DragonSpark.Windows
{
	[ApplyAutoValidation, ApplyInverseSpecification( typeof(UserSettingsExistsSpecification) )]
	public sealed class InitializeUserSettingsCommand : CommandBase<ApplicationSettingsBase>
	{
		[UsedImplicitly]
		public static InitializeUserSettingsCommand Default { get; } = new InitializeUserSettingsCommand();
		InitializeUserSettingsCommand() : this( TemplatesFactory.Implementation.WithParameter( SystemLogger.Default.Get ).Get, Defaults.UserSettingsPath, SaveUserSettingsCommand.Default.Execute ) {}

		readonly Func<Templates> templatesSource;
		readonly Func<IFileInfo> fileSource;
		readonly Action<ApplicationSettingsBase> save;

		[UsedImplicitly]
		public InitializeUserSettingsCommand( Func<Templates> templatesSource, Func<IFileInfo> fileSource, Action<ApplicationSettingsBase> save )
		{
			this.templatesSource = templatesSource;
			this.fileSource = fileSource;
			this.save = save;
		}

		public override void Execute( ApplicationSettingsBase parameter )
		{
			var templates = templatesSource();
			var file = fileSource();
			var name = file.FullName;

			templates.Initializing( name );

			parameter.Upgrade();

			var command = file.Refreshed().Exists ? templates.Complete : templates.NotFound;
			command( name );
			
			if ( !file.Exists )
			{
				try
				{
					save( parameter );
					templates.Created( name );
				}
				catch ( ConfigurationErrorsException e )
				{
					templates.ErrorSaving( e, name );
				}
			}
		}

		sealed class TemplatesFactory : ParameterizedSourceBase<ILogger, Templates>
		{
			public static TemplatesFactory Implementation { get; } = new TemplatesFactory();
			TemplatesFactory() {}

			public override Templates Get( ILogger parameter ) => new Templates( InitializingTemplate.Defaults.Get( parameter ).Execute,
																				 NotFoundTemplate.Defaults.Get( parameter ).Execute,
																				 ErrorSavingTemplate.Defaults.Get( parameter ).Execute,
																				 CreatedTemplate.Defaults.Get( parameter ).Execute,
																				 CompleteTemplate.Defaults.Get( parameter ).Execute
																				 );

			sealed class InitializingTemplate : LogCommandBase<string>
			{
				public static IParameterizedSource<ILogger, InitializingTemplate> Defaults { get; } = new Cache<ILogger, InitializingTemplate>( logger => new InitializingTemplate( logger ) );
				InitializingTemplate( ILogger logger ) : base( logger, Resources.LoggerTemplates_Initializing ) {}
			}

			sealed class ErrorSavingTemplate : LogExceptionCommandBase<string>
			{
				public static IParameterizedSource<ILogger, ErrorSavingTemplate> Defaults { get; } = new Cache<ILogger, ErrorSavingTemplate>( logger => new ErrorSavingTemplate( logger ) );
				ErrorSavingTemplate( ILogger logger ) : base( logger.Warning, Resources.LoggerTemplates_ErrorSaving ) {}
			}

			sealed class NotFoundTemplate : LogCommandBase<string>
			{
				public static IParameterizedSource<ILogger, NotFoundTemplate> Defaults { get; } = new Cache<ILogger, NotFoundTemplate>( logger => new NotFoundTemplate( logger ) );
				NotFoundTemplate( ILogger logger ) : base( logger, Resources.LoggerTemplates_NotFound ) {}
			}

			sealed class CreatedTemplate : LogCommandBase<string>
			{
				public static IParameterizedSource<ILogger, CreatedTemplate> Defaults { get; } = new Cache<ILogger, CreatedTemplate>( logger => new CreatedTemplate( logger ) );
				CreatedTemplate( ILogger logger ) : base( logger, Resources.LoggerTemplates_Created ) {}
			}

			sealed class CompleteTemplate : LogCommandBase<string>
			{
				public static IParameterizedSource<ILogger, CompleteTemplate> Defaults { get; } = new Cache<ILogger, CompleteTemplate>( logger => new CompleteTemplate( logger ) );
				CompleteTemplate( ILogger logger ) : base( logger, Resources.LoggerTemplates_Complete ) {}
			}
		}

		public struct Templates
		{
			public Templates( Action<string> initializing, Action<string> notFound, Action<Exception, string> errorSaving, Action<string> created, Action<string> complete )
			{
				Initializing = initializing;
				NotFound = notFound;
				ErrorSaving = errorSaving;
				Created = created;
				Complete = complete;
			}

			public Action<string> Initializing { get; }
			public Action<string> NotFound { get; }
			public Action<Exception, string> ErrorSaving { get; }
			public Action<string> Created { get; }
			public Action<string> Complete { get; }
		}
	}
}