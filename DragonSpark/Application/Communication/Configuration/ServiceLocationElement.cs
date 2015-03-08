using System;
using System.ComponentModel;
using System.Configuration;
using System.ServiceModel.Configuration;
using DragonSpark.Objects;
using Microsoft.Practices.ServiceLocation;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Communication.Configuration
{
	public class ServiceLocationElement : BehaviorExtensionElement
	{
		public override Type BehaviorType
		{
			get { return typeof(ServiceLocationBehavior); }
		}

		protected override object CreateBehavior()
		{
			var result = new ServiceLocationBehavior( () => Locator );
			return result;
		}

	    IServiceLocator Locator
	    {
	        get { return locator ?? ( locator = ResolveLocator() ); }
	    }	IServiceLocator locator;

	    IServiceLocator ResolveLocator()
		{
			var factory = Activator.CreateInstance<Factory<IServiceLocator>>( FactoryType );
			return factory.Create();
		}

		[ConfigurationProperty( ServiceLocatorTypeName, DefaultValue = null, IsRequired = true, IsKey = false ), TypeConverter( typeof(TypeNameConverter) )]
		public Type FactoryType
		{
			get { return (Type)this[ ServiceLocatorTypeName ]; }
			set { this[ ServiceLocatorTypeName ] = value; }
		}	const string ServiceLocatorTypeName = "factoryType";
	}
}