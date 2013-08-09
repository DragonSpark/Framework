using Microsoft.Practices.Unity;
using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Parameter" )]
	public class InjectionProperty : InjectionMember
	{
		public string PropertyName { get; set; }

		[TypeConverter( typeof(InjectionParameterValueTypeConverter) )]
		public InjectionParameterValue Parameter { get; set; }

		public override Microsoft.Practices.Unity.InjectionMember Create( IUnityContainer container, Type targetType )
		{
			var type = targetType.GetProperty( PropertyName ).PropertyType;

			var parameter = Parameter.Create( type );
			var result = new Microsoft.Practices.Unity.InjectionProperty( PropertyName, parameter );
			return result;
		}
	}
}