using System;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation
{
	[Singleton( typeof(IApplicationContext), Priority = Priority.Lowest )]
	public class ApplicationContext : IApplicationContext
	{
		public Uri Location
		{
			get { return System.Windows.Application.Current.GetUri(); }
		}
	}
}