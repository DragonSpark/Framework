using System;
using System.IO;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Io
{
	public class SubdirectoryFactory : FactoryBase
	{
		public string DirectoryBuildKey { get; set; }

		public string DirectoryPath { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var directory = container.Resolve<DirectoryInfo>( DirectoryBuildKey );
			var result = directory.CreateSubdirectory( DirectoryPath );
			return result;
		}
	}
}