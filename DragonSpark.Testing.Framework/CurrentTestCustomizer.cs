using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public class CurrentTestCustomizer : ICustomization
	{
		readonly MethodInfo methodUnderTest;
		readonly Type[] customizationTypes;

		public CurrentTestCustomizer( MethodInfo methodUnderTest, Type[] customizationTypes )
		{
			this.methodUnderTest = methodUnderTest;
			this.customizationTypes = customizationTypes;
		}

		public void Customize( IFixture fixture )
		{
			var items = DetermineCustomizations( fixture );
			items.Apply( customization => fixture.Customize( customization ) );
		}

		protected virtual ICustomization[] DetermineCustomizations( IFixture fixture )
		{
			
			var type = methodUnderTest.DeclaringType;
			var customizations = DefaultCustomizations();
			var items = customizations.Concat( 
				type.Transform( t => t.Assembly.GetCustomAttributes() )
					.Concat( type.Transform( t => t.GetCustomAttributes() ) )
					.Concat( methodUnderTest.GetCustomAttributes() )
					.OfType<ICustomization>().Prioritize() 
				)
				.ToArray();
			return items;
		}

		protected virtual ICustomization[] DefaultCustomizations()
		{
			var result = new ICustomization[] { AmbientCustomizationsCustomization.Instance, new CurrentMethodCustomization( methodUnderTest ), new TypesCustomization( customizationTypes ) };
			return result;
		}
	}
}