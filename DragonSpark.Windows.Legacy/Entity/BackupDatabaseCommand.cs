using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Linq;
using Path = DragonSpark.Windows.FileSystem.Path;

namespace DragonSpark.Windows.Legacy.Entity
{
	public sealed class BackupDatabaseCommand : CommandBase<object>
	{
		readonly Func<IFileInfo, bool> lockedSource;
		readonly Func<string> pathSource;
		readonly Func<string, bool> validSource;
		readonly IPath path;

		public BackupDatabaseCommand() : this( LockedFileSpecification.Default.IsSatisfiedBy, TimestampNameFactory.Default.Get, TimestampNameSpecification.Default.IsSatisfiedBy, Path.Default ) {}

		public BackupDatabaseCommand( Func<IFileInfo, bool> lockedSource, Func<string> pathSource, Func<string, bool> validSource, IPath path )
		{
			this.lockedSource = lockedSource;
			this.pathSource = pathSource;
			this.validSource = validSource;
			this.path = path;
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
					file.CopyTo( path.Combine( destination.FullName, file.Name ) );
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