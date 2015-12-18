using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	class DefaultValueProvider : IDefaultValueProvider
	{
		readonly object value;

		public DefaultValueProvider( object value )
		{
			this.value = value;
		}

		public virtual object GetValue( DefaultValueParameter parameter )
		{
			var result = value.ConvertTo( parameter.Metadata.PropertyType );
			return result;
		}
	}
}