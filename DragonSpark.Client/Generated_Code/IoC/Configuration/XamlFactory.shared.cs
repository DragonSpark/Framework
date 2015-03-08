using System;
using System.IO;
using DragonSpark.Serialization;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class XamlFactory : FactoryBase
	{
		public string FilePath { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			using ( var fileStream = File.OpenRead( FilePath ) )
			{
				var result = XamlSerializationHelper.Load( fileStream );
				return result;
			}
		}
	}
}