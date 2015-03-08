using System;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Instance" )]
	public class TypeInstanceValue : InstanceValue
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type TypeInstance
		{
			get { return Instance.To<Type>(); }
			set { Instance = value; }
		}
	}
}