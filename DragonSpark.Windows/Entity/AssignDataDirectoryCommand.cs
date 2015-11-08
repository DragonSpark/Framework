using System;
using System.IO;
using DragonSpark.ComponentModel;
using DragonSpark.Setup;

namespace DragonSpark.Windows.Entity
{
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
}