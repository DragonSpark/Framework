﻿using System.IO;
using DragonSpark.Diagnostics;
using Microsoft.Practices.Unity;

namespace DragonSpark.Server.Legacy.IoC.Io
{
	public class DeleteIfExists : UnityContainerTypeConfiguration
	{
		protected override void Configure(IUnityContainer container, UnityType type)
		{
			container.Resolve<FileInfo>( type.BuildName ).NotNull( x => x.Exists.IsTrue( () => Log.Try( x.Delete ) ) );
			base.Configure(container, type);
		}
	}
}