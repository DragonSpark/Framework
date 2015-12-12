using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
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
			var current = Activator.Current.Activate<FactoryBuiltObjectFactory>();
			var result = current.Create( new ObjectFactoryParameter( type, FactoryReflectionSupport.Instance.GetResultType( type ) ?? info.PropertyType ) );
			return result;
		}
	}
}