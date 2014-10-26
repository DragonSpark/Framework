using System;
using System.IO;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Io
{
	public class FileInfoFactory : FactoryBase
	{
		public string FilePath { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = new FileInfo( FilePath );
			return result;
		}
	}
}