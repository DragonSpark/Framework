﻿using DragonSpark.ComponentModel;
using DragonSpark.Setup;
using DragonSpark.Windows.Markup;

namespace DragonSpark.Windows.Setup
{
	public class InitializeMarkupCommand : SetupCommand
	{
		[Singleton]
		public MarkupExtensionMonitor Monitor { get; set; }

		protected override void Execute( ISetupParameter parameter )
		{
			Monitor.Initialize();
		}
	}
}