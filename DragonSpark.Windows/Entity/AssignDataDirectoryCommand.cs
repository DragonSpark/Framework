using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Windows.Io;
using DragonSpark.Windows.Properties;
using DragonSpark.Windows.Runtime;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows.Entity
{
	public static class EntityFiles
	{
		public static DirectoryInfo DefaultDataDirectory { get; } = new DirectoryInfo( @".\App_Data" );

		public static IEnumerable<FileInfo> WithLog( FileInfo databaseFile )
		{
			var result = new[] { databaseFile, GetLog( databaseFile ) };
			return result;
		}

		public static FileInfo GetLog( FileInfo database )
		{
			var result = new FileInfo( Path.Combine( database.DirectoryName, string.Concat( Path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
			return result;
		}
	}

	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class DataDirectoryPath : AppDomainValue<string>
	{
		public const string Key = "DataDirectory";

		public DataDirectoryPath() : base( Key )
		{}
	}

	public class AssignDataDirectoryCommand : SetupCommand
	{
		[ComponentModel.Singleton( typeof(EntityFiles), nameof(EntityFiles.DefaultDataDirectory) )]
		public DirectoryInfo Directory { get; set; }

		[Activate]
		public DataDirectoryPath Path { get; set; }

		protected override void Execute( SetupContext context )
		{
			Path.Assign( Directory.FullName );
		}
	}

	public class InstallDatabaseCommand : SetupCommand
	{
		[Factory( typeof(AttachedDatabaseFileFactory) )]
		public FileInfo Database { get; set; }

		protected override void Execute( SetupContext context )
		{
			Database.Exists.IsFalse( () =>
			{
				var items = EntityFiles.WithLog( Database ).TupleWith( new[] { Resources.Blank, Resources.Blank_log } );
				items.Apply( tuple => 
				{
					using ( var stream = File.Create( tuple.Item1.FullName ) )
					{
						stream.Write( tuple.Item2, 0, tuple.Item2.Length );
					}
				} );
			} );
		}
	}

	public class AttachedDatabaseFileFactory : Factory<FileInfo>
	{
		readonly DbContext context;

		public AttachedDatabaseFileFactory( DbContext context )
		{
			this.context = context;
		}

		protected override FileInfo CreateFrom( Type resultType, object parameter )
		{
			var result = new SqlConnectionStringBuilder( context.Database.Connection.ConnectionString ).AttachDBFilename.NullIfEmpty().Transform( DbProviderServices.ExpandDataDirectory ).Transform( s => new FileInfo( s ) );
			return result;
		}
	}

	public class BackupDatabaseCommand : SetupCommand
	{
		[Factory( typeof(AttachedDatabaseFileFactory) )]
		public FileInfo Database { get; set; }

		[Default( 6 )]
		public int? MaximumBackups { get; set; }

		protected override void Execute( SetupContext context )
		{
			Database.With( file =>
			{
				var files = EntityFiles.WithLog( Database ).Where( info => !info.IsLocked() ).ToArray();
				files.Any().IsTrue( () =>
				{
					var destination = file.Directory.CreateSubdirectory( FileSystem.GetValidPath() );
					files.Apply( info => info.CopyTo( Path.Combine( destination.FullName, info.Name ) ) );
				} );

				MaximumBackups.WithValue( i => 
					file.Directory
						.GetDirectories()
						.Where( x => FileSystem.IsValidPath( x.Name ) )
						.OrderByDescending( info => info.CreationTime )
						.Skip( i )
						.Apply( info => info.Delete( true ) ) 
				);
			} );
		}
	}
}