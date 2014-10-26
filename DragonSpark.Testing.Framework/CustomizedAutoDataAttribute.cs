using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public class CustomizedAutoDataAttribute : Ploeh.AutoFixture.Xunit.AutoDataAttribute
	{
		readonly Type[] cutomizationTypes;

		public CustomizedAutoDataAttribute( params Type[] cutomizationTypes )
		{
			this.cutomizationTypes = cutomizationTypes;
		}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest, Type[] parameterTypes )
		{
			var instances =  cutomizationTypes./*Where( x => typeof(ICustomization).IsAssignableFrom( x ) ).*/Select( Activator.CreateInstance ).ToArray();
			instances.OfType<ICustomization>().Apply( x => Fixture.Customize( x ) );
			var customizations = instances.OfType<IMethodCustomization>().ToArray();
			
			customizations.Apply( x => x.Customizing( methodUnderTest, parameterTypes ) );

			var result = base.GetData( methodUnderTest, parameterTypes );
			
			customizations.Apply( x => x.Customized( methodUnderTest, parameterTypes, result ) );
			
			return result;
		}
	}
}