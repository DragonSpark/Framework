using DragonSpark.IoC.Configuration;
using DragonSpark.Server.IO;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.IoC.Io
{
	public class WorkspaceFactory : FactoryBase
	{
		public string WorkspacePath { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = Workspace.Create( WorkspacePath );
			return result;
		}
	}
}