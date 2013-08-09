using System;
using System.IO;
using DragonSpark.Io;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Io
{
	public class WorkspaceDirectoryFactory : FactoryBase
	{
		public string WorkspaceName { get; set; }

		public string DirectoryPath { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var workspace = container.Resolve<IWorkspace>( WorkspaceName );
			var path = Path.Combine( workspace.Directory.FullName, DirectoryPath );
			var result = Directory.CreateDirectory( path );
			return result;
		}
	}
}