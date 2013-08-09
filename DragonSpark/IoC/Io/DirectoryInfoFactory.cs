using System;
using System.IO;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Io
{
	public class DirectoryInfoFactory : FactoryBase
	{
		public string DirectoryPath { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = Directory.CreateDirectory( DirectoryPath );
			return result;
		}
	}
}