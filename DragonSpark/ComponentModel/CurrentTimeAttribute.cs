using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.ComponentModel
{
	public sealed class CurrentTimeAttribute : DefaultValueBase
	{
		public CurrentTimeAttribute() : base( t => Activator.Activate<CurrentTimeValueProvider>() ) {}
	}

	public class CurrentTimeValueProvider : IDefaultValueProvider
	{
		readonly ICurrentTime currentTime;

		public CurrentTimeValueProvider() : this( CurrentTime.Instance ) {}

		public CurrentTimeValueProvider( [Required]ICurrentTime currentTime )
		{
			this.currentTime = currentTime;
		}

		public object GetValue( DefaultValueParameter parameter ) => parameter.Metadata.PropertyType == typeof(DateTime) ? (object)currentTime.Now.DateTime : currentTime.Now;
	}
}