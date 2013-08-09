using System;
using System.ComponentModel;
using System.Configuration;

namespace DragonSpark.Configuration
{
	[ConfigurationSettingsName( ConfigurationProviderSection.Name )]
	class ConfigurationProviderSection : ConfigurationSection
	{
		const string Name = "configurationProvider";

		[ConfigurationProperty( ProviderTypeName, DefaultValue = null, IsRequired = false, IsKey = false ), TypeConverter( typeof(TypeNameConverter) )]
		public Type ProviderType
		{
			get { return (Type)this[ ProviderTypeName ]; }
			set { this[ ProviderTypeName ] = value; }
		}	const string ProviderTypeName = "providerType";
	}
}
