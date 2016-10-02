using DragonSpark.Application;
using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Windows
{
	public sealed class AppDomainFormatter : IFormattable
	{
		readonly string title;

		public AppDomainFormatter( [UsedImplicitly]AppDomain appDomain ) : this( DefaultAssemblyInformationSource.Default.Get().Title ) {}

		AppDomainFormatter( string title )
		{
			this.title = title;
		}

		public string ToString( [Optional]string format, [Optional]IFormatProvider formatProvider ) => title;
	}
}