using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Reflection;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Objects
{
	public class FactoryDefaultValueAttribute : ActivationDefaultAttribute
	{
		public FactoryDefaultValueAttribute( Type type ) : base( type )
		{}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var value = base.GetValue( instance, propertyInfo ).AsTo<IFactory,object>( x => x.Create( propertyInfo.PropertyType ) );
			return value;
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Io", Justification = "It's not Io." ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1813:AvoidUnsealedAttributes" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Not meant for public access." ), AttributeUsage( AttributeTargets.Property )]
	public class ActivationDefaultAttribute : DefaultPropertyValueAttribute
	{
		readonly Type type;

		public ActivationDefaultAttribute( Type type )
		{
			this.type = type;
		}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			try
			{
				var result = Activator.CreateInstance<object>( type );
				return result;
			}
			catch ( ActivationException )
			{
				return null;
			}
		}
	}
}