using DragonSpark.Runtime;
using System;

namespace DragonSpark.ComponentModel
{
	public sealed class CurrentTimeAttribute : DefaultValueBase
	{
		public CurrentTimeAttribute() : base( () => new CurrentTimeValueProvider() )
		{}
	}

	public class CurrentTimeValueProvider : IDefaultValueProvider
	{
		readonly ICurrentTime currentTime;

		public CurrentTimeValueProvider() : this( CurrentTime.Instance )
		{}

		public CurrentTimeValueProvider( ICurrentTime currentTime )
		{
			this.currentTime = currentTime;
		}

		public object GetValue( DefaultValueParameter parameter )
		{
			var result = parameter.Metadata.PropertyType == typeof(DateTime) ? (object)currentTime.Now.DateTime : currentTime.Now;
			return result;
		}
	}
}