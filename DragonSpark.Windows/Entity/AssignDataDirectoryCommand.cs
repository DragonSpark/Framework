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

	public class BackupDatabaseCommand : SetupCommand
	{
		[Activate]
		public DbContext Context { get; set; }

		[Default( 6 )]
		public int? MaximumBackups { get; set; }

		protected override void Execute( SetupContext context )
		{
			new SqlConnectionStringBuilder( Context.Database.Connection.ConnectionString ).AttachDBFilename.With( s => DbProviderServices.ExpandDataDirectory( s ).With( file =>
			{
				var directory = new DirectoryInfo( Path.GetDirectoryName( file ) );
				
				var files = new[] { file, Path.Combine( directory.FullName, string.Concat( Path.GetFileNameWithoutExtension( file ), "_log.ldf" ) ) }.Select( name => new FileInfo( name ) ).Where( info => !info.IsLocked() ).ToArray();
				files.Any().IsTrue( () =>
				{
					var destination = directory.CreateSubdirectory( FileSystem.GetValidPath() );
					files.Apply( info => info.CopyTo( Path.Combine( destination.FullName, info.Name ) ) );
				} );

				MaximumBackups.WithValue( i => 
					directory
						.GetDirectories()
						.Where( x => FileSystem.IsValidPath( x.Name ) )
						.OrderByDescending( info => info.CreationTime )
						.Skip( i )
						.Apply( info => info.Delete( true ) ) 
				);
			} ) );
		}
	}
}