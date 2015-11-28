using DragonSpark.Activation;
using DragonSpark.Setup;
using System;
using System.Reflection;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.ComponentModel
{
	public sealed class FactoryAttribute : ActivateAttribute
	{
		public FactoryAttribute( Type activatedType ) : base( activatedType )
		{}

		protected override object Activate( object instance, PropertyInfo info, Type type, string s )
		{
			var result = Activator.Current.Activate<FactoryBuiltObjectFactory>().Create( new ObjectFactoryParameter( type, info.PropertyType ) );
			return result;
		}
	}
}