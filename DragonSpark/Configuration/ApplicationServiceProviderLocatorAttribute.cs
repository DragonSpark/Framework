using System;
using System.ComponentModel;
using System.Configuration;

namespace DragonSpark.Configuration
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class ApplicationServiceProviderLocatorAttribute : Attribute
	{
		[TypeConverter( typeof(TypeNameConverter) )]
		public Type ProviderType { get; set; }
	}
}