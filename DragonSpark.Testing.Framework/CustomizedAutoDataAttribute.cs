using System.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Testing.Framework
{
	public class CustomizedAutoDataAttribute : AutoDataAttribute
	{
		readonly Type[] cutomizationTypes;

		public CustomizedAutoDataAttribute( params Type[] cutomizationTypes )
		{
			this.cutomizationTypes = cutomizationTypes;
		}

		static IEnumerable<T> Determine<T>( IEnumerable<Type> types )
		{
			var result = types.Where( typeof(T).IsAssignableFrom ).Select( Activator.CreateInstance<T> ).ToArray();
			return result;
		}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest, Type[] parameterTypes )
		{
			var type = methodUnderTest.DeclaringType;
			var customizers = type.Assembly.GetCustomAttributes()
				.Concat( type.GetCustomAttributes() )
				.Concat( methodUnderTest.GetCustomAttributes() ).OfType<ICustomization>();
			var instances = Determine<ICustomization>( cutomizationTypes ).Concat( customizers ).Prioritize().ToArray();

			instances.Apply( customization => customization.Customize( Fixture ) );

			/*var customizations = instances.OfType<IMethodCustomization>().ToArray();
			customizations.Apply( x => x.Customizing( methodUnderTest, parameterTypes ) );
			customizations.Apply( x => x.Customized( methodUnderTest, parameterTypes, result ) );*/
			
			var result = base.GetData( methodUnderTest, parameterTypes );
			return result;
		}
	}
}