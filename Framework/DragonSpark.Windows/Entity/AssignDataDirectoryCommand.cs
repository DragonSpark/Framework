using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Windows.Io;
using Microsoft.Practices.Unity;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Windows.Properties;

namespace DragonSpark.Windows.Entity
{
	public interface IValue<out T>
	{
		T Item { get; }
	}

	public interface IWritableValue<T> : IValue<T>
	{
		void Assign( T item );
	}

	public abstract class Value<T> : IValue<T>
	{
		
		public abstract T Item { get;  }
	}

	public abstract class WritableValue<T> : Value<T>, IWritableValue<T>
	{
		public abstract void Assign( T item );
	}

	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class DataDirectory : WritableValue<string>
	{
		public const string DataDirectoryKey = "DataDirectory", DefaultPath = @".\App_Data";

		public static FileInfo GetLog( FileInfo database )
		{
			var result = new FileInfo( Path.Combine( database.DirectoryName, string.Concat( Path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
			return result;
		}

		public DataDirectory( [Dependency( DataDirectoryKey )] DirectoryInfo directory )
		{
			Assign( directory.FullName );
		}

		public override void Assign( string item )
		{
			AppDomain.CurrentDomain.SetData( DataDirectoryKey, item );
		}

		public override string Item => (string)AppDomain.CurrentDomain.GetData( DataDirectoryKey );
	}

	public class AssignDataDirectoryCommand : SetupCommand
	{
		[Activate( DataDirectory.DataDirectoryKey )]
		public DirectoryInfo Directory { get; set; }

		[Activate]
		public DataDirectory DataDirectory { get; set; }

		protected override void Execute( SetupContext context )
		{
			DataDirectory.Item.Null( () => DataDirectory.Assign( Directory.FullName ) );
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
				var items = new[]
				{
					new Tuple<FileInfo, byte[]>( Database, Resources.Blank ),
					new Tuple<FileInfo, byte[]>( DataDirectory.GetLog( Database ), Resources.Blank_log )
				};

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
				var files = new[] { file, DataDirectory.GetLog( file ) }.Where( info => !info.IsLocked() ).ToArray();
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