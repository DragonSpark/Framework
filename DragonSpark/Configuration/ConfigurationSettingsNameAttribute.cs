using System;
using System.Collections.Generic;
using System.Text;

namespace DragonSpark.Configuration
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class ConfigurationSettingsNameAttribute : Attribute
	{
		public ConfigurationSettingsNameAttribute( string name )
		{
			if ( string.IsNullOrEmpty( name ) )
			{
				throw new ArgumentNullException( "name" );
			}

			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}	readonly string name;
	}
}