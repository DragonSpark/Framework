using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using System;
using System.IO;
using Microsoft.Practices.Unity;

namespace DragonSpark.Windows.Entity
{
	public interface IDataDirectoryContext
	{
		void Assign( DirectoryInfo directory );

		DirectoryInfo Value { get; }
	}

	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	class DataDirectoryContext : IDataDirectoryContext
	{
		public void Assign( DirectoryInfo directory )
		{
			Value = directory;
		}

		public DirectoryInfo Value { get; private set; }
	}

	public class AssignDataDirectoryCommand : SetupCommand
	{
		public const string DataDirectory = "DataDirectory";

		[Default( @".\App_Data" )]
		public string Path { get; set; }

		protected override void Execute( SetupContext context )
		{
			var name = Directory.CreateDirectory( System.IO.Path.GetFullPath( Path ) ).FullName;
			AppDomain.CurrentDomain.SetData( DataDirectory, name );
		}
	}

	public class BackupDatabaseCommand : SetupCommand
	{
		public DirectoryInfo DataDirectory { get; set; }

		protected override void Execute( SetupContext context )
		{
			DataDirectory.With( info =>
			{

			} );
		}
	}
}