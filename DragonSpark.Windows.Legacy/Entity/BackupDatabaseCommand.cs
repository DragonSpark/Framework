using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows.Legacy.Entity
{
	public sealed class BackupDatabaseCommand : CommandBase<object>
	{
		public static BackupDatabaseCommand Default { get; } = new BackupDatabaseCommand();
		BackupDatabaseCommand() : this( LockedFileSpecification.Default.IsSatisfiedBy, TimestampPathFactory.Default.Get, TimestampPathSpecification.Default.IsSatisfiedBy ) {}

		readonly Func<IFileInfo, bool> lockedSource;
		readonly Func<string> pathSource;
		readonly Func<string, bool> validSource;

		public BackupDatabaseCommand( Func<IFileInfo, bool> lockedSource, Func<string> pathSource, Func<string, bool> validSource )
		{
			this.lockedSource = lockedSource;
			this.pathSource = pathSource;
			this.validSource = validSource;
		}

		[Service, PostSharp.Patterns.Contracts.NotNull, UsedImplicitly]
		public IFileInfo Database { [return: PostSharp.Patterns.Contracts.NotNull]get; set; }

		[Default( 6 ), PostSharp.Patterns.Contracts.NotNull, UsedImplicitly]
		public int? MaximumBackups { get; set; }

		public override void Execute( object parameter )
		{
			var directory = Database.Directory;
			var files = EntityFiles.WithLog( Database ).Where( lockedSource ).ToArray();
			if ( files.Any() )
			{
				var destination = directory.CreateSubdirectory( pathSource() );
				foreach ( var file in files )
				{
					file.CopyTo( Path.Combine( destination.FullName, file.Name ) );
				}
			}

			if ( MaximumBackups.HasValue )
			{
				directory
					.GetDirectories()
					.Where( x => validSource( x.Name ) )
					.OrderByDescending( info => info.CreationTime )
					.Skip( MaximumBackups.Value )
					.Each( info => info.Delete( true ) );
			}
		}
	}
}